using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;


public class StateChangeEventArgs: EventArgs{
    public String NewState {get; set;}
}



/// <summary>
/// The manager for the whole game main task is to start and stop scenes and levels
/// </summary>
public class GameManager : MonoBehaviour {

    

    



    private const int MAIN_MENU_SCENE_INDEX = 0;
    private const int TEST_LEVEL_SCENE_INDEX = 1;
    
    private static GameManager instance;

    private String currentState = "";


    // -- properties -- //

    /// <summary>
    /// Setting up singleton pattern
    /// </summary>
    public static GameManager Instance {
        get {
            if (instance == null) {
                GameObject gameObject = new GameObject("GameManager");
                gameObject.AddComponent<GameManager>();
            }

            return instance;
        }
    }

    // -- public -- //

    /// <summary>
    /// Decrements the number of enemies by one and then checks if there is any left
    /// If there are none left it will fire the OnEndGame event, and then show the MainMenu
    /// </summary>
    // public void DecrementEnemies() {
    //     numberOfEnemies--;
    //     if (numberOfEnemies <= 0) {
    //         OnEndGame?.Invoke();
    //         ShowMainMenu();
    //     }
    // }

    // -- events -- //    

    /// <summary>
    /// This event tells the listeners the game state has changed
    /// </summary>
    public static event EventHandler<StateChangeEventArgs> GameStateChangeEvent;

    private void SubscribeToEvents() {
        // todo subscribe to OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        LevelManager.LevelStateChangeEvent += c_LevelStateChangeEvent;
           
    }
    
    private void UnsubscribeFromEvents() {
        // todo unsubscribe from OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        // maybe that this also should be done on disable
        LevelManager.LevelStateChangeEvent -= c_LevelStateChangeEvent;
        
    }

    private void c_LevelStateChangeEvent(object o, StateChangeEventArgs args){
        if (args.NewState == "EXIT"){
            this.GameStateChange("MAIN_MENU");
        }
    }

    

    // -- private -- //

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        // Finds out how many enemies are in the level
        //numberOfEnemies = levelDetails.NumberOfEnemies;
    }


    

    public void GameStateChange(String NewState){

        this.currentState = NewState;
        StateChangeEventArgs args = new StateChangeEventArgs();
        args.NewState = NewState;
        GameStateChangeEvent?.Invoke(this, args);

        switch (NewState)
        {
            case "MAIN_MENU":
                Debug.Log($"New state : MAIN_MENU");
                UnityEngine.SceneManagement.SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
                break;

            case "TEST_LEVEL":
                Debug.Log($"New state : TEST_LEVEL");
                SceneManager.Instance.ChangeScene(TEST_LEVEL_SCENE_INDEX);
                break;

            default:
                Debug.Log("UNKNOWN GAME STATE");
                break;
        }

        
    }





    // -- unity -- //

    private void OnEnable() {
        SubscribeToEvents();
    }
    
    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

}