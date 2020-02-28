using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Singleton<PauseMenu> {
    [SerializeField]
    private GameObject pauseMenuCanvas;

    [SerializeField]
    private GameObject confirmDialog;

    private Boolean isPaused = false;

    [SerializeField]
    private Button continueButton;

    private void Awake() {
        CheckForMissingComponents();

        DeactivateMenu();
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
    }

    private void DeactivateConfirmDialog() {
        confirmDialog.SetActive(false);
    }

    private void ActivateConfirmDialog() {
        confirmDialog.SetActive(true);
    }

    private void ResumeGame() {
        LevelManager.Instance.ChangeLevelState(LEVEL_STATE.PLAY);
        DeactivateMenu();
    }

    private void PauseGame() {
        LevelManager.Instance.ChangeLevelState(LEVEL_STATE.PAUSE);
        ActivateMenu();
    }

    private void ActivateMenu() {
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
        }
    }
}