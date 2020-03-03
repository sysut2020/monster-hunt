using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The event data for the pause menu changed events 
/// </summary>
public class PauseMenuChangeEventArgs : EventArgs {
    public LEVEL_STATE NewLevelState { get; set; }
}

/// <summary>
/// Handler for Pause menu. Turns on and of the main pause menu and the quit confirmation when needed.
/// Can tells the LevelManager when to switch to Pause/Play state.
/// </summary>
public class PauseMenuController : Singleton<PauseMenuController> {
    [SerializeField]
    private GameObject pauseMenuCanvas;

    [SerializeField]
    private GameObject confirmDialog;

    private Boolean isPaused = false;

    [SerializeField]
    private Button continueButton;

    private void Awake() {
        CheckForMissingComponents();

        SubscribeToEvents();
        DeactivateMenu();
    }

    private void CallbackChangePauseMenuState(object _, ButtonClickEventArgs args) {
        if (args.ButtonEvent.GetType() == typeof(PAUSE_MENU_STATE)) {
            ChangePauseMenuState((PAUSE_MENU_STATE) args.ButtonEvent);
        }
    }

    private void Start() {
        continueButton.onClick.AddListener(ResumeGame);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (confirmDialog.activeSelf) {
                DeactivateConfirmDialog();
            } else {
                TogglePause();
            }
        }
    }

    /// <summary>
    /// Toggles the pause state.
    /// Shows pause menu if paused
    /// </summary>
    private void TogglePause() {
        if (!isPaused) {
            PauseGame();
            isPaused = true;
        } else {
            ResumeGame();
            isPaused = false;
        }
    }

    private void OnDestroy() {
        continueButton.onClick.RemoveAllListeners();
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        PauseMenuButton.buttonEventHandler += CallbackChangePauseMenuState;
    }

    private void UnsubscribeFromEvents() {
        PauseMenuButton.buttonEventHandler -= CallbackChangePauseMenuState;
    }

    private void DeactivateConfirmDialog() {
        confirmDialog.SetActive(false);
    }

    private void ActivateConfirmDialog() {
        confirmDialog.SetActive(true);
    }

    private void ResumeGame() {
        PauseMenuChangeEventArgs args = new PauseMenuChangeEventArgs();
        args.NewLevelState = LEVEL_STATE.PLAY;
        PauseMenuChangeEvent?.Invoke(this, args);
        DeactivateMenu();
    }

    private void PauseGame() {
        PauseMenuChangeEventArgs args = new PauseMenuChangeEventArgs();
        args.NewLevelState = LEVEL_STATE.PAUSE;
        PauseMenuChangeEvent?.Invoke(this, args);
        ActivateMenu();
    }

    private void ActivateMenu() {
        pauseMenuCanvas.SetActive(true);
    }

    private void DeactivateMenu() {
        pauseMenuCanvas.SetActive(false);
    }

    /// <summary>
    /// Checks that all necessary components are given to the script
    /// </summary>
    /// <exception cref="MissingComponentException"></exception>
    private void CheckForMissingComponents() {
        if (pauseMenuCanvas == null) {
            throw new MissingComponentException("Missing pause menu canvas");
        }

        if (confirmDialog == null) {
            throw new MissingComponentException("Missing confirm dialog");
        }
    }

    /// <summary>
    /// Changes the state of the pause menu in the case where the player wants to quit the game
    /// </summary>
    /// <param name="pauseState"></param>
    public void ChangePauseMenuState(PAUSE_MENU_STATE pauseState) {
        switch (pauseState) {
            case PAUSE_MENU_STATE.CONFIRMATION:
                ActivateConfirmDialog();
                break;

            case PAUSE_MENU_STATE.BASE:
                DeactivateConfirmDialog();
                break;

            case PAUSE_MENU_STATE.QUIT:
                // Sending quit signal to listeners
                PauseMenuChangeEventArgs args = new PauseMenuChangeEventArgs();
                args.NewLevelState = LEVEL_STATE.EXIT;
                PauseMenuChangeEvent?.Invoke(this, args);
                break;

            default:
                Debug.LogWarning("State unknown for Pause Menu");
                break;
        }
    }

    /// <summary>
    /// This event tells the listeners the level state should change
    /// </summary>
    public static event EventHandler<PauseMenuChangeEventArgs> PauseMenuChangeEvent;
}