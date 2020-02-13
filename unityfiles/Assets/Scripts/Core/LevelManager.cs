using System;
using UnityEngine;


/// <summary>
/// The temporary data needed while playing throgh the level
/// </summary>
class PlayThroughData{
    public int EnemysKilled {get; set;}
}


/// <summary>
/// A manager for a level in the game 
/// </summary>
public class LevelManager : MonoBehaviour {
    [SerializeField]
    private GameObject gameOverCanvas;


    [SerializeField]
    private LevelDetails levelDetails;

    private int numberOfEnemies = 0;


    private Timers levelTimer = new Timers();
    private String currentState = "WAITING";

    private PlayThroughData playThroughData;
    


    // -- singelton -- //

    
    private static LevelManager instance;

    public static LevelManager Instance{
        get {
            if (instance == null) {
                instance = new LevelManager();
            }

            return instance;
        }
    }

    private void Awake(){
        instance = (LevelManager.Instance != null)? new LevelManager(): LevelManager.Instance;
    }

    // -- properties -- //

    // -- public -- //

    // -- events -- //


    /// <summary>
    /// This event tells the listeners the level state has changed
    /// </summary>
    public static event EventHandler<StateChangeEventArgs> LevelStateChangeEvent;

    /// <summary>
    /// This event tells the listeners they are about to be deleted and shold relese 
    /// all subscribed events
    /// </summary>
    public static event EventHandler CleanUpEvent;


    private void SubscribeToEvents(){
        CleanUpEvent             += UnsubscribeFromEvents;
        Player.PlayerKilledEvent += c_PlayerKilledEvent;
        Enemy.EnemyKilledEvent   += c_EnemyKilledEvent;
        GameManager.GameStateChangeEvent += c_GameStateChangeEvent;
    }

    private void UnsubscribeFromEvents(){
        CleanUpEvent -= UnsubscribeFromEvents;

        Player.PlayerKilledEvent -= c_PlayerKilledEvent;
        Enemy.EnemyKilledEvent   -= c_EnemyKilledEvent;
        GameManager.GameStateChangeEvent -= c_GameStateChangeEvent;
    }

    private void c_PlayerKilledEvent(object o, EventArgs _){
        LevelStateChange("GAME_OVER");
    }

    private void c_EnemyKilledEvent(object o, EnemyEventArgs args){
        playThroughData.EnemysKilled += 1;
        if (this.levelDetails.NumberOfEnemies <= playThroughData.EnemysKilled){
            this.LevelStateChange("EXIT");
        }
    }

    private void c_GameStateChangeEvent(object o, StateChangeEventArgs args){
        if (args.NewState == "TEST_LEVEL"){
                this.LevelStateChange("HUNTING");
        }
    }
    

    private void LevelStateChange(String NewState){

        this.currentState = NewState;
        StateChangeEventArgs args = new StateChangeEventArgs();
        args.NewState = NewState;
        LevelStateChangeEvent?.Invoke(this, args);
        
        switch (NewState)
        {
            /// The game is over show game over screen
            case "WAITING":
                Debug.Log($"New state : WAITING");
                
                break;
            /// The game is over show game over screen
            case "GAME_OVER":
                Debug.Log($"New state : GAME_OVER");
                gameOverCanvas.SetActive(true);
                break;

            /// Start the main mode spawn the player and start the level
            case "HUNTING":
                Debug.Log($"New state : HUNTING");
                Spawner.Instance.SpawnOnAll();
                Player.Instance.gameObject.transform.position = new Vector3(levelDetails.spawnPoint.x,levelDetails.spawnPoint.y, 0);

                break;

            /// Exit the game and go to main menu
            case "EXIT":
                Debug.Log($"New state : EXIT");
                break;

            default:
                Debug.Log("UNKNOWN LEVEL STATE");
                break;
        }
    }


    private void UnsubscribeFromEvents(object o, EventArgs _) => this.UnsubscribeFromEvents();

    // -- private -- //

    


    /// <summary>
    /// Triggers an event telling the listeners they are about to be deleted and
    /// and should unsubscribe from all events osv
    /// </summary>
    private void CleanUpScene(){
        CleanUpEvent?.Invoke(this, EventArgs.Empty);
    }

    private void InitScene(){
        CleanUpEvent?.Invoke(this, EventArgs.Empty);
    }


    private void startLevelTime(){
        this.levelTimer.Set("LEVEL_TIME", this.levelDetails.Time);
        
    }




    // -- unity -- //

    private void Start(){
        this.playThroughData = new PlayThroughData();
        this.startLevelTime();
        this.LevelStateChange("HUNTING");
    }
    private void Update(){

        if (this.levelTimer.Done("LEVEL_TIME")){
            this.LevelStateChange("EXIT");
        }
    }

    private void OnEnable() {
        SubscribeToEvents();
    }
    
    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

}