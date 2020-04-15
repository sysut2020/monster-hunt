using System;
using UnityEngine;

/// <summary>
/// The event data for the game state changed events 
/// </summary>
public class GameStateChangeEventArgs : EventArgs {
    public GAME_STATE NewState { get; set; }
    public int NextSceneIndex { get; set; }
}

/// <summary>
/// The manager for the whole game main task is to start and stop scenes and levels
/// </summary>
public class GameManager : Singleton<GameManager> {

    private GAME_STATE currentState;
    private int nextSceneIndex = 0;

    /// <summary>
    /// Index of all levels in the game
    /// </summary>
    private readonly SCENE_INDEX[] levels = {
        SCENE_INDEX.LEVEL1, 
        SCENE_INDEX.LEVEL2,
        SCENE_INDEX.LEVEL3,
        SCENE_INDEX.LEVEL4,
        SCENE_INDEX.LEVEL5
    };

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
    
    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void SubscribeToEvents() {
        // todo subscribe to OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        LevelManager.OnLevelStateChangeEvent += CallbackLevelStateChangeEvent;
        LetterGameManager.OnLetterGameEndedEvent += CallbackLetterGameEnded;
    }

    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void UnsubscribeFromEvents() {
        // todo unsubscribe from OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        // maybe that this also should be done on disable
        LevelManager.OnLevelStateChangeEvent -= CallbackLevelStateChangeEvent;
        LetterGameManager.OnLetterGameEndedEvent -= CallbackLetterGameEnded;
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

    /// <summary>
    /// This function is fired when OnLetterGameEndedEvent is invoked
    /// This function will trigger on:
    ///     LetterGameManager OnDestroy()
    ///         the letter game is done and the total score from the letter
    ///         game level is transmitted with the event 
    /// </summary>
    /// <param name="args">the event args containing the total score from letter level</param>
    private void CallbackLetterGameEnded(object _, LetterGameEndedArgs args) {
        if (args.Score > 0) {
            gameDataManager.AddGameScore(args.Score);
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
                nextSceneIndex = 0; //resets game
                SceneManager.Instance.ChangeScene(SCENE_INDEX.MAIN_MENU);
                break;

            case GAME_STATE.START_GAME:
                // reset counter just in case
                nextSceneIndex = 1;
                SceneManager.Instance.ChangeScene(SCENE_INDEX.LEVEL1);
                break;
            
            // todo add a countinue state where one can play from the level last played
            
            case GAME_STATE.LETTER_LEVEL:
                SceneManager.Instance.ChangeScene(SCENE_INDEX.LETTER_GAME);
                break;

            case GAME_STATE.SCOREBOARD:
                SceneManager.Instance.ChangeScene(SCENE_INDEX.SCOREBOARD);
                break;

            case GAME_STATE.EXIT:
                Application.Quit();
                break;
            
            case GAME_STATE.NEXT_LEVEL:
                if (nextSceneIndex >= levels.Length) { // no more levels
                    this.GameStateChange(GAME_STATE.SCOREBOARD);
                    break;
                }
                var nextScene = levels[nextSceneIndex];
                nextSceneIndex++;
                args.NextSceneIndex = nextSceneIndex;
                SceneManager.Instance.ChangeScene(nextScene);
                
                break;

            default:
                Debug.LogError("🌮🌮🌮🌮  UNKNOWN GAME STATE  🌮🌮🌮🌮");
                break;
        }

        Debug.Log(nextSceneIndex);
        GameStateChangeEvent?.Invoke(this, args);
    }


    // -- unity -- //

    private void Awake() { 
        this.gameDataManager = new GameDataManager();
        SubscribeToEvents();
    }
    

    private void OnDestroy() {
        UnsubscribeFromEvents();
        this.gameDataManager.SaveData();
    }
}
