using System;
using UnityEngine;

/// <summary>
/// The event data for the game state changed events 
/// </summary>
public class LevelStateChangeEventArgs : EventArgs {
    public LEVEL_STATE NewState { get; set; }
}

/// <summary>
/// The temporary data needed while playing through the level
/// </summary>
class PlayThroughData {
    public int EnemysKilled { get; set; }
    public int LetterCollected { get; set; }
}

/// <summary>
/// A manager for a level in the game 
/// </summary>
public class HuntingLevelController : Singleton<HuntingLevelController> {

    [SerializeField]
    private LevelDetails levelDetails;

    public LevelDetails LevelDetails { get { return this.levelDetails; } private set { this.levelDetails = value; } }

    private PlayerInventory playerInventory;
    private WUTimers levelTimer = new WUTimers();
    private string LEVEL_TIMER_ID;
    private LEVEL_STATE currentState; // may need default here in that case find out the starting state

    private PlayThroughData playThroughData;
    private GameDataManager dataManager;

    // -- properties -- //

    // -- public -- //

    // -- events -- //

    /// <summary>
    /// This event tells the listeners the level state has changed
    /// </summary>
    public static event EventHandler<LevelStateChangeEventArgs> OnLevelStateChangeEvent;

    /// <summary>
    /// This event tells the listeners they are about to be deleted and should relese 
    /// all subscribed events
    /// Mainly for non mono behaviour objects that cant use onDelete
    /// </summary>
    public static event EventHandler CleanUpEvent;

    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void SubscribeToEvents() {
        LetterCollectable.OnLetterCollected += CallbackLetterCollected;
        PlayerHealthController.OnPlayerLivesUpdate += CallbackPlayerLivesUpdate;
        Enemy.EnemyKilledEvent += CallbackEnemyKilledEvent;
        GameManager.GamePausedEvent += CallbackOnGamePaused;
    }

    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void UnsubscribeFromEvents() {
        LetterCollectable.OnLetterCollected -= CallbackLetterCollected;
        PlayerHealthController.OnPlayerLivesUpdate -= CallbackPlayerLivesUpdate;
        Enemy.EnemyKilledEvent -= CallbackEnemyKilledEvent;
        GameManager.GamePausedEvent -= CallbackOnGamePaused;
    }

    private void CallbackLetterCollected(object _, LetterCollectedArgs args) {
        playThroughData.LetterCollected++;
        if (this.playThroughData.LetterCollected == this.levelDetails.NumberOfLetters) {
            this.ChangeLevelState(LEVEL_STATE.GAME_WON);
        }
    }

    /// <summary>
    /// This function is fiered when the PlayerKilled is invoked
    /// Ends the level
    /// </summary>
    /// <param name="_">the object calling</param>
    /// <param name="args">the event args</param>
    private void CallbackPlayerLivesUpdate(object _, PlayerLivesUpdateArgs args) {
        if (args.CurrentLives == 0) LevelStateChange(LEVEL_STATE.GAME_OVER);
    }

    /// <summary>
    /// This function is fiered when the EnemyKilled is invoked
    /// Increses the enemy killed counter by one
    /// </summary>
    /// <param name="_">the object calling</param>
    /// <param name="args">the event args</param>
    private void CallbackEnemyKilledEvent(object _, EnemyEventArgs args) {
        playThroughData.EnemysKilled++;
        if (this.levelDetails.NumberOfEnemies <= playThroughData.EnemysKilled) {
            this.LevelStateChange(LEVEL_STATE.GAME_WON);
        }
    }

    /// <summary>
    /// Called when the game is paused/unpaused.
    /// </summary>
    /// <param name="_">the object that sent the event > unused</param>
    /// <param name="args">event arguments</param>
    private void CallbackOnGamePaused(object _, GamePausedEventArgs args) {
        if (args.IsPaued) {
            this.levelTimer.Pause(this.LEVEL_TIMER_ID);
        } else {
            this.levelTimer.Contue(this.LEVEL_TIMER_ID);
        }
    }

    /// <summary>
    /// Changes the level state to the provided state
    /// </summary>
    /// <param name="state">state to change too</param>
    public void ChangeLevelState(LEVEL_STATE state) {
        Instance.LevelStateChange(state);
    }

    /// <summary>
    /// Changes the level state 
    /// </summary>
    /// <param name="NewState">The new level state</param>
    private void LevelStateChange(LEVEL_STATE NewState) {
        this.currentState = NewState;
        LevelStateChangeEventArgs args = new LevelStateChangeEventArgs();
        args.NewState = NewState;
        switch (NewState) {
            // The game is over show game over screen
            case LEVEL_STATE.GAME_OVER:
            case LEVEL_STATE.GAME_WON:
                break;
            case LEVEL_STATE.PLAY:
                InitLevel();
                break;
            default:
                Debug.Log("🌮🌮🌮🌮  UNKNOWN LEVEL STATE  🌮🌮🌮🌮");
                break;
        }

        OnLevelStateChangeEvent?.Invoke(this, args);
    }

    // TODO: maybe remove?
    /// <summary>
    /// spawns all the enemys and inits the player
    /// </summary>
    private void InitLevel() {
        EntitySpawner.Instance.MaxSpawns = this.levelDetails.NumberOfEnemies;
        EntitySpawner.Instance?.Init(this.levelDetails.NumberOfEnemiesAtStart); // Init with X mobs on the map
    }

    /// <summary>
    /// Triggers an event telling the listeners they are about to be deleted and
    /// and should unsubscribe from all events osv
    /// </summary>
    private void CleanUpScene() {
        CleanUpEvent?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Starts the level timer
    /// </summary>
    private void startLevelTime() {
        this.levelTimer.Set(LEVEL_TIMER_ID, this.levelDetails.Time);
    }

    /// <summary>
    /// returns the time left in milliseconds
    /// if timer is done -1 is returned
    /// </summary>
    /// <returns>time left in milliseconds -1 if done</returns>
    public int GetLevelTimeLeft() {
        return this.levelTimer.TimeLeft(this.LEVEL_TIMER_ID);
    }

    // -- unity -- //

    private void Start() {
        dataManager = GameManager.Instance.GameDataManager;
        playerInventory = new PlayerInventory();
        this.playThroughData = new PlayThroughData();
        LEVEL_TIMER_ID = this.levelTimer.RollingUID;
        this.startLevelTime();
        this.LevelStateChange(LEVEL_STATE.PLAY);
    }

    private void Update() {
        if (this.levelTimer.Done(LEVEL_TIMER_ID)) {
            this.LevelStateChange(LEVEL_STATE.GAME_OVER);
        }
    }

    private void OnEnable() {
        SubscribeToEvents();
    }

    private void OnDestroy() {
        CleanUpScene();
        UnsubscribeFromEvents();

        dataManager.AddLetters(playerInventory.CollectedLetters);
        dataManager.AddMoney(playerInventory.Money);
    }
}