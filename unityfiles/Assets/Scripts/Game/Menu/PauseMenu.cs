using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Singleton<PauseMenu> {
    [SerializeField]
    private GameObject pauseMenuCanvas;

    [SerializeField]
    private GameObject confirmDialog;

    private Boolean isPaused = false;

    private Button continueButton;
    private static readonly int PAUSE = 0;
    private static readonly float PLAY = 1;

    private void Awake() {
        CheckForMissingComponents();
        
        DeactivateMenu(); 
    }

    private void Start() {
        SubscribeToEvents();
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
        isPaused = !isPaused;

        if (isPaused) {
            PauseGame();
        } else {
            ResumeGame();
        }
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents() {
        LevelManager.LevelStateChangeEvent -= ChangeGameState();
    }

    private void SubscribeToEvents() {
        LevelManager.LevelStateChangeEvent += ChangeGameState();
    }

    private EventHandler<LevelStateChangeEventArgs> ChangeGameState() {
        return (sender, args) => {
            if (args.NewState == LEVEL_STATE.PAUSE) {
                PauseGame();
            }

            if (args.NewState == LEVEL_STATE.PLAY) {
                // If PLAY is called from anywhere, make sure to turn off all canvases
                DeactivateConfirmDialog();
                ResumeGame();
            }
        };
    }

    private void DeactivateConfirmDialog() {
        confirmDialog.SetActive(false);
    }

    private void ActivateConfirmDialog() {
        confirmDialog.SetActive(true);
    }

    private void ResumeGame() {
        Time.timeScale = PLAY;
        isPaused = false;
        DeactivateMenu();
    }

    private void PauseGame() {
        isPaused = true;
        Time.timeScale = PAUSE;
        ActivateMenu();
    }

    void ActivateMenu() {
        pauseMenuCanvas.SetActive(true);
    }

    public void DeactivateMenu() {
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

    public void ChangePauseMenuState(PAUSE_MENU_STATE pauseState) {
        switch (pauseState) {
            case PAUSE_MENU_STATE.CONFIRMATION:
                ActivateConfirmDialog();
                break;
            
            case PAUSE_MENU_STATE.BASE:
                DeactivateConfirmDialog();
                break;
        }
    }
}