using UnityEngine;

/// <summary>
/// Responsible for passing the hunting level game time to a timer gui instance,
/// which is responsible for displaying time.
/// </summary>
public class LevelTimeToGuiHandler : MonoBehaviour {

    /// <summary>
    /// Reference to a TimerGUI instance which is resposible for displating time.
    /// </summary>
    private TimerGUI timerGUI;

    private void Start() {
        if (TryGetComponent(out TimerGUI timerGuiSript)) {
            this.timerGUI = timerGuiSript;
        } else {
            throw new MissingComponentException("Cant find timer GUI");
        }
    }

    private void Update() {
        this.timerGUI.SetTime(HuntingLevelController.Instance.GetLevelTimeLeft());
    }

}