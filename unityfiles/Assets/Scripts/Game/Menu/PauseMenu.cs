using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
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

            if (args.NewState == LEVEL_STATE.RESUME) {
                if (confirmDialog.activeSelf) {
                    DeactivateConfirmDialog();
                } else {
                    ResumeGame();
                }
            }

            if (args.NewState == LEVEL_STATE.CONFIRM) {
                ActivateConfirmDialog();
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

    private void CheckForMissingComponents() {
        if (pauseMenuCanvas == null) {
            throw new MissingComponentException("Missing pause menu canvas");
        }

        if (confirmDialog == null) {
            throw new MissingComponentException("Missing confirm dialog");
        }
    }
}