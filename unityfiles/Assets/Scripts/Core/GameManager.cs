using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class GameManager : MonoBehaviour {
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
    /// </summary>
    public void DecrementEnemies() {
        numberOfEnemies--;
        if (numberOfEnemies <= 0) {
            OnEndGame?.Invoke();
        }
    }

    private void OnDestroy() {
        // todo unsubscribe from OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        // maybe that this also should be done on disable
        Enemy.OnEnemyDead -= DecrementEnemies;
        Player.OnPlayerDead -= ShowGameOver;
    }

    public void ShowGameOver() {
        // todo show game over
        gameOverCanvas.SetActive(true);
    }


    //-- Events --//

    public delegate void EndGameHandler();

    public event EndGameHandler OnEndGame;
}