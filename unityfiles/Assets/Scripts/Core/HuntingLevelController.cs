using System;
using UnityEngine;

/// <summary>
/// The event arguments for the game state changed events 
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
    private string LEVEL_TIMER_ID = "";
    private LEVEL_STATE currentState; // may need default here in that case find out the starting state

    private PlayThroughData playThroughData;

    /// <summary>
    /// This event tells the listeners the level state has changed
    /// </summary>
    public static event EventHandler<LevelStateChangeEventArgs> OnLevelStateChangeEvent;

    /// <summary>
    /// This event tells the listeners they are about to be deleted and should release 
    /// all subscribed events
    /// Mainly for non mono behaviour objects that cant use onDestroy
    /// </summary>
    public static event EventHandler CleanUpEvent;

    private void SubscribeToEvents() {
        LetterCollectible.OnLetterCollected += CallbackLetterCollected;
        PlayerHealthController.OnPlayerLivesUpdate += CallbackPlayerLivesUpdate;
        Enemy.EnemyKilledEvent += CallbackEnemyKilledEvent;
        GameManager.GamePausedEvent += CallbackOnGamePaused;
    }

    private void UnsubscribeFromEvents() {
        LetterCollectible.OnLetterCollected -= CallbackLetterCollected;
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
    /// Fired when the PlayerKilled event is invoked
    /// Ends the level
    /// </summary>
    /// <param name="_">the object calling</param>
    /// <param name="args">the event args</param>
    private void CallbackPlayerLivesUpdate(object _, PlayerLivesUpdateArgs args) {
        if (args.CurrentLives == 0) LevelStateChange(LEVEL_STATE.GAME_OVER);
    }

    /// <summary>
    /// This function is fired when the EnemyKilled event is invoked
    /// Increases the enemy killed counter by one
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
    /// Called when the game is paused/un paused.
    /// </summary>
    /// <param name="_">the object that sent the event > unused</param>
    /// <param name="args">event arguments</param>
    private void CallbackOnGamePaused(object _, GamePausedEventArgs args) {
        if (args.IsPaused) {
            this.levelTimer.Pause(this.LEVEL_TIMER_ID);
        } else {
            this.levelTimer.Continue(this.LEVEL_TIMER_ID);
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
                break;
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
    /// Returns the time left in milliseconds
    /// </summary>
    /// <returns>Time left in milliseconds, if no timer is attached to ID -1 is returned.</returns>
    public int GetLevelTimeLeft() {
        return this.levelTimer.TimeLeft(this.LEVEL_TIMER_ID);
    }

    private void Start() {
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
        GameManager.Instance.GameDataManager?.AddLetters(playerInventory.CollectedLetters);
        GameManager.Instance.GameDataManager?.AddMoney(playerInventory.Money);
    }
}