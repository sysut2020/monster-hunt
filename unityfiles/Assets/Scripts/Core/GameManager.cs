using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;

    [SerializeField]
    private GameObject gameOverCanvas;
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
        }  else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable() {
        // todo subscribe to OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        Player.OnPlayerDead += ShowGameOver;
    }

    private void OnDestroy() {
        // todo unsubscribe from OnPlayerDead, OnTimeOut, OnAllEnemiesDead
        // maybe that this also should be done on disable
        Player.OnPlayerDead -= ShowGameOver;
    }

    public void ShowGameOver() {
        // todo show game over
        gameOverCanvas.SetActive(true);   
        Debug.Log("Player died");
    }


    //-- Events --//

    public delegate void EndGameHandler();

    public event EndGameHandler OnEndGame;
}