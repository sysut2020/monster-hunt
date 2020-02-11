using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;

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
    
    //-- Events --//

    public delegate void OnEndGame();

    public event OnEndGame OnEndGame;
}