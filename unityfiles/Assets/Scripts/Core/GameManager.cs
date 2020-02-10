public class GameManager {
    private static GameManager instance;

    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = new GameManager();
            }

            return instance;
        }
    }
    
    
    //-- Events --//

    public delegate void OnEndGame();

    public event OnEndGame OnEndGame;
}