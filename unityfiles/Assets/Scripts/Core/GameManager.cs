using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class GameManager : MonoBehaviour {
    private const int MAIN_MENU_SCENE_INDEX = 0;

    
    private static GameManager instance;

    [SerializeField]
    private GameObject gameOverCanvas;

    [SerializeField]
    private LevelDetails levelDetails;

    private int numberOfEnemies = 0;

    public static GameManager Instance {
        get {
            if (instance == null) {
                GameObject gameObject = new GameObject("GameManager");
                gameObject.AddComponent<GameManager>();
            }

            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        numberOfEnemies = levelDetails.numberOfEnemies;
    }

    private void OnEnable() {
        // todo subscribe to OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        Enemy.OnEnemyDead += DecrementEnemies;
        Player.OnPlayerDead += ShowGameOver;
    }

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

    private void OnDestroy() {
        // todo unsubscribe from OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        // maybe that this also should be done on disable
        Enemy.OnEnemyDead -= DecrementEnemies;
        Player.OnPlayerDead -= ShowGameOver;
    }

    private void ShowGameOver() {
        // todo show game over
        gameOverCanvas.SetActive(true);
    }
    
    private void ShowMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
    }


    //-- Events --//

    public delegate void EndGameHandler();

    public static event EndGameHandler OnEndGame;
}