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
            this.DeactivateConfirmDialog();
            isPaused = false;
        }
    }

    private void DeactivateConfirmDialog() {
        confirmDialog.SetActive(false);
    }

    private void ResumeGame() {
        DeactivateMenu();
    }

    private void PauseGame() {
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
    }

}