using System;
using UnityEngine;



/// <summary>
/// A manager for a level in the game 
/// </summary>
public class LevelManager : MonoBehaviour {
    // -- singelton -- //

    [SerializeField]
    private GameObject gameOverCanvas;
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
    public static event EventHandler CleanUpEvent;


    private void SubscribeToEvents(){
        CleanUpEvent += UnsubscribeFromEvents;
        Player.PlayerKilledEvent += c_PlayerKilledEvent;
    }

    private void UnsubscribeFromEvents(){
        CleanUpEvent -= UnsubscribeFromEvents;
    }

    private void c_PlayerKilledEvent(object o, EventArgs _){
        LevelStateChange("GAME_OVER")
    }

    private void UnsubscribeFromEvents(object o, EventArgs _) => this.UnsubscribeFromEvents();

    private void LevelStateChange(String NewState){
        switch (NewState)
        {
            case "GAME_OVER":
                Debug.Log($"New state : GAME_OVER");
                gameOverCanvas.SetActive(true);
                break;

            case "HUNTING":
                Debug.Log($"New state : HUNTING");
                break;

            default:
                Debug.Log("UNKNOWN LEVEL STATE");
                break;
        }
    }

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

}