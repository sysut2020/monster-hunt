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
}

/// <summary>
/// A manager for a level in the game 
/// </summary>
public class LevelManager : Singleton<LevelManager> {
    [SerializeField]
    private GameObject gameOverCanvas;

    [SerializeField]
    private GameObject gameWonCanvas;

    [SerializeField]
    private LevelDetails levelDetails;

    private Timers levelTimer = new Timers();
    private string LEVEL_TIMER_ID;
    private LEVEL_STATE currentState; // may need default here in that case find out the starting state

    private PlayThroughData playThroughData;
    private static readonly int PAUSE = 0;
    private static readonly int PLAY = 1;

    // -- properties -- //

    // -- public -- //

    // -- events -- //

    /// <summary>
    /// This event tells the listeners the level state has changed
    /// </summary>
    public static event EventHandler<LevelStateChangeEventArgs> LevelStateChangeEvent;

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
        Player.PlayerKilledEvent += CallbackPlayerKilledEvent;
        Enemy.EnemyKilledEvent += CallbackEnemyKilledEvent;
    }

    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void UnsubscribeFromEvents() {
        Player.PlayerKilledEvent -= CallbackPlayerKilledEvent;
        Enemy.EnemyKilledEvent -= CallbackEnemyKilledEvent;
    }

    /// <summary>
    /// This function is fiered when the PlayerKilled is invoked
    /// Ends the level
    /// </summary>
    /// <param name="o">the object calling</param>
    /// <param name="args">the event args</param>
    private void CallbackPlayerKilledEvent(object o, EventArgs _) {
        LevelStateChange(LEVEL_STATE.GAME_OVER);
    }

    /// <summary>
    /// This function is fiered when the EnemyKilled is invoked
    /// Increses the enemy killed counter by one
    /// </summary>
    /// <param name="o">the object calling</param>
    /// <param name="args">the event args</param>
    private void CallbackEnemyKilledEvent(object o, EnemyEventArgs args) {
        playThroughData.EnemysKilled += 1;
        if (this.levelDetails.NumberOfEnemies <= playThroughData.EnemysKilled) {
            this.LevelStateChange(LEVEL_STATE.GAME_WON);
        }
    }

    /// <summary>
    /// Changes the level state to the provided state
    /// </summary>
    /// <param name="state">state to change too</param>
    public void ChangeLevelState(LEVEL_STATE state) {
        LevelManager.Instance.LevelStateChange(state);
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
            /// The game is over show game over screen
            case LEVEL_STATE.GAME_OVER:
                Time.timeScale = PAUSE;
                gameOverCanvas.SetActive(true);
                break;
            case LEVEL_STATE.GAME_WON:
                Time.timeScale = PAUSE;
                gameWonCanvas.SetActive(true);
                break;

            case LEVEL_STATE.PAUSE:
                Time.timeScale = PAUSE;
                break;
            
            case LEVEL_STATE.START:
                InitLevel();
                ChangeLevelState(LEVEL_STATE.PLAY);
                break;
                
            /// Start the main mode spawn the player and start the level
            case LEVEL_STATE.PLAY:
                Time.timeScale = PLAY;
                break;
            /// Exit the game and go to main menu
            case LEVEL_STATE.EXIT:
                break;

            case LEVEL_STATE.RELOAD:
                Time.timeScale = PLAY;
                CleanUpEvent.Invoke(this, EventArgs.Empty);
                SceneManager.Instance.RestartCurrentScene();
                break;

            default:
                Debug.Log("🌮🌮🌮🌮  UNKNOWN LEVEL STATE  🌮🌮🌮🌮");
                break;
        }

        LevelStateChangeEvent?.Invoke(this, args);
    }

    // -- private -- //

    // TODO: maybe remove?
    /// <summary>
    /// spawns all the enemys and inits the player
    /// </summary>
    private void InitLevel() {
        Spawner.Instance?.SpawnOnAll();
    }

    /// <summary>
    /// Triggers an event telling the listeners they are about to be deleted and
    /// and should unsubscribe from all events osv
    /// </summary>
    private void CleanUpScene() {
        CleanUpEvent?.Invoke(this, EventArgs.Empty);
    }

    private void startLevelTime() {
        this.levelTimer.Set(LEVEL_TIMER_ID, this.levelDetails.Time);
    }

    // -- unity -- //

    private void Start() {
        this.playThroughData = new PlayThroughData();
        LEVEL_TIMER_ID = this.levelTimer.RollingUID;
        this.startLevelTime();

        this.LevelStateChange(LEVEL_STATE.START);
    }

    private void Update() {
        if (this.levelTimer.Done(LEVEL_TIMER_ID)) {
            this.LevelStateChange(LEVEL_STATE.EXIT);
        }
    }

    public int GetLevelTimeLeft(){
        return this.levelTimer.TimeLeft(this.LEVEL_TIMER_ID);
    }

    private void OnEnable() {
        SubscribeToEvents();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }
}