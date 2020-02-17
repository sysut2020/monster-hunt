using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;


public class StateChangeEventArgs: EventArgs{
    public STATE NewState {get; set;}
}



/// <summary>
/// The manager for the whole game main task is to start and stop scenes and levels
/// </summary>
public class GameManager : Singleton<GameManager> {

    private const int MAIN_MENU_SCENE_INDEX = 0;
    private const int TEST_LEVEL_SCENE_INDEX = 1;
    
    private static GameManager instance;

    private STATE currentState;


    // -- properties -- //

    // -- public -- //

 
    // -- events -- //    

    /// <summary>
    /// This event tells the listeners the game state has changed
    /// </summary>
    public static event EventHandler<StateChangeEventArgs> GameStateChangeEvent;


    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void SubscribeToEvents() {
        // todo subscribe to OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        LevelManager.LevelStateChangeEvent += c_LevelStateChangeEvent;
           
    }
    
    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void UnsubscribeFromEvents() {
        // todo unsubscribe from OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        // maybe that this also should be done on disable
        LevelManager.LevelStateChangeEvent -= c_LevelStateChangeEvent;
        
    }

    /// <summary>
    /// This function is fiered when the LevelStateChangeEvent is invoked
    /// This function will trigger on the following level states:
    /// 
    /// STATE.EXIT: 
    ///     the level is done and the game state should be changed to MAIN_MENU
    /// </summary>
    /// <param name="o">the object calling (this should always be the level manager)</param>
    /// <param name="args">the event args containing the new state</param>
    private void c_LevelStateChangeEvent(object o, StateChangeEventArgs args){
        if (args.NewState == STATE.EXIT){
            this.GameStateChange(STATE.MAIN_MENU);
        }
    }

    

    // -- private -- //
    
    /// <summary>
    /// Changes the game state 
    /// </summary>
    /// <param name="NewState">The new game state</param>
    public void GameStateChange(STATE NewState){

        this.currentState = NewState;
        StateChangeEventArgs args = new StateChangeEventArgs();
        args.NewState = NewState;
        

        switch (NewState)
        {
            case STATE.MAIN_MENU:
                UnityEngine.SceneManagement.SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
                break;

            case STATE.TEST_LEVEL:
                SceneManager.Instance.ChangeScene(TEST_LEVEL_SCENE_INDEX);
                break;

            case STATE.EXIT_GAME:
                Application.Quit();
                break;

            default:
                Debug.Log("🌮🌮🌮🌮  UNKNOWN GAME STATE  🌮🌮🌮🌮");
                break;
        }

        GameStateChangeEvent?.Invoke(this, args);

        
    }





    // -- unity -- //

    private void OnEnable() {
        SubscribeToEvents();
    }
    
    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

}