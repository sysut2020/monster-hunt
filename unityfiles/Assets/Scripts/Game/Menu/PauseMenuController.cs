using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handler for Pause menu. Turns on and of the main pause menu and the quit confirmation when needed.
/// Can tells the LevelManager when to switch to Pause/Play state.
/// </summary>
public class PauseMenuController : MonoBehaviour {

    [SerializeField]
    private GameObject pauseMenuCanvas;

    [SerializeField]
    private GameObject pauseMenuUIElement;

    [SerializeField]
    private GameObject confirmDialog;

    [SerializeField]
    private GameObject howToPlay;

    private Boolean isPaused = false;

    private void Awake() {
        CheckForMissingComponents();
        DeactivateMenu();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (confirmDialog.activeSelf) {
                DeactivateConfirmDialog();
            } else if (howToPlay.activeSelf) {
                DeactivateHowToPlay();
            } else {
                TogglePause();
            }
        }
    }

    /// <summary>
    /// Toggles the pause state.
    /// Shows pause menu if paused
    /// </summary>
    public void TogglePause() {
        if (!isPaused) {
            PauseGame();
            isPaused = true;
        } else {
            ResumeGame();
            DeactivateConfirmDialog();
            DeactivateHowToPlay();
            isPaused = false;
        }
    }

    private void DeactivateHowToPlay() {
        howToPlay.SetActive(false);
        pauseMenuUIElement.SetActive(true);
    }

    private void DeactivateConfirmDialog() {
        confirmDialog.SetActive(false);
    }

    private void ResumeGame() {
        GameManager.Instance.SetGameState(GAME_STATE.PLAY);
        DeactivateMenu();
    }

    private void PauseGame() {
        GameManager.Instance.SetGameState(GAME_STATE.PAUSE);
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

        if (pauseMenuUIElement == null) {
            throw new MissingComponentException("Missing pause menu ui element");
        }

        if (confirmDialog == null) {
            throw new MissingComponentException("Missing confirm dialog");
        }

        if (howToPlay == null) {
            throw new MissingComponentException("Missing how to play screen");
        }
    }

}