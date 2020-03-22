using System;
using UnityEngine;

/// <summary>
/// The event data for the game state changed events 
/// </summary>
public class GameStateChangeEventArgs : EventArgs {
    public GAME_STATE NewState { get; set; }
}

/// <summary>
/// The manager for the whole game main task is to start and stop scenes and levels
/// </summary>
public class GameManager : Singleton<GameManager> {
    private const int MAIN_MENU_SCENE_INDEX = 0;
    private const int TEST_LEVEL_SCENE_INDEX = 1;
    private const int LETTER_GAME_SCENE_INDEX = 2;

    private GAME_STATE currentState;

    private GameDataManager gameDataManager;

    // -- properties -- //
    public GameDataManager GameDataManager {
        get => gameDataManager;
    }

    // -- public -- //

    // -- events -- //    

    /// <summary>
    /// This event tells the listeners the game state has changed
    /// </summary>
    public static event EventHandler<GameStateChangeEventArgs> GameStateChangeEvent;

    public static event EventHandler OnMainMenuMusic;
    public static event EventHandler OnLevel1Music;

    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void SubscribeToEvents() {
        // todo subscribe to OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        LevelManager.OnLevelStateChangeEvent += CallbackLevelStateChangeEvent;
    }

    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void UnsubscribeFromEvents() {
        // todo unsubscribe from OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        // maybe that this also should be done on disable
        LevelManager.OnLevelStateChangeEvent -= CallbackLevelStateChangeEvent;
    }

    /// <summary>
    /// This function is fired when the OnLevelStateChangeEvent is invoked
    /// This function will trigger on the following level states:
    /// 
    /// STATE.EXIT: 
    ///     the level is done and the game state should be changed to MAIN_MENU
    /// </summary>
    /// <param name="o">the object calling (this should always be the level manager)</param>
    /// <param name="args">the event args containing the new state</param>
    private void CallbackLevelStateChangeEvent(object o, LevelStateChangeEventArgs args) {
        if (args.NewState == LEVEL_STATE.EXIT) {
            this.GameStateChange(GAME_STATE.MAIN_MENU);
        }
    }

    // -- private -- //

    /// <summary>
    /// Changes the game state 
    /// </summary>
    /// <param name="NewState">The new game state</param>
    public void GameStateChange(GAME_STATE NewState) {
        this.currentState = NewState;
        GameStateChangeEventArgs args = new GameStateChangeEventArgs();
        args.NewState = NewState;
        
        switch (NewState) {
            case GAME_STATE.MAIN_MENU:
                UnityEngine.SceneManagement.SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
                break;

            case GAME_STATE.TEST_LEVEL:
                SceneManager.Instance.ChangeScene(TEST_LEVEL_SCENE_INDEX);
                break;
            
            case GAME_STATE.LETTER_LEVEL:
                SceneManager.Instance.ChangeScene(LETTER_GAME_SCENE_INDEX);
                
                break;

            case GAME_STATE.EXIT:
                Application.Quit();
                break;

            default:
                Debug.LogError("🌮🌮🌮🌮  UNKNOWN GAME STATE  🌮🌮🌮🌮");
                break;
        }

        Debug.Log("Game event invoked");
        GameStateChangeEvent?.Invoke(this, args);
    }


    // -- unity -- //

    private void Awake() { 
        DontDestroyOnLoad(this);
        this.gameDataManager = new GameDataManager();
        SubscribeToEvents();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
        this.gameDataManager.SaveData();
    }

    private void PlayMainMenuMusic() {
        OnMainMenuMusic?.Invoke(this, EventArgs.Empty);
    }

    private void PlayLevel1Music() {
        OnLevel1Music?.Invoke(this, EventArgs.Empty);
        Debug.Log("Eventhandler called");
    }
}