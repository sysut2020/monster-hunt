using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;





/// <summary>
/// The manager for the whole game main task is to start and stop scenes and levels
/// </summary>
public class GameManager : MonoBehaviour {

    

    [SerializeField]
    private LevelDetails levelDetails;



    private const int MAIN_MENU_SCENE_INDEX = 0;
    
    private static GameManager instance;

    private int numberOfEnemies = 0;

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
    public void DecrementEnemies() {
        numberOfEnemies--;
        if (numberOfEnemies <= 0) {
            OnEndGame?.Invoke();
            ShowMainMenu();
        }
    }

    // -- events -- //    
    private void SubscribeToEvents() {
        // todo subscribe to OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        Enemy.OnEnemyDead += DecrementEnemies;
        Player.OnPlayerDead += ShowGameOver;        
    }

    

    private void UnsubscribeFromEvents() {
        // todo unsubscribe from OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        // maybe that this also should be done on disable
        Enemy.OnEnemyDead -= DecrementEnemies;
        Player.OnPlayerDead -= ShowGameOver;
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
        numberOfEnemies = levelDetails.NumberOfEnemies;
    }

    private void OnEnable() {
        SubscribeToEvents();
    }
    
    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    

    private void GameStateChange(String NewState){
        switch (NewState)
        {
            case "MAIN_MENU":
                Debug.Log($"New state : MAIN_MENU");
                UnityEngine.SceneManagement.SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
                break;

            default:
                Debug.Log("UNKNOWN GAME STATE");
                break;
        }
    }





    //-- Events --//

}