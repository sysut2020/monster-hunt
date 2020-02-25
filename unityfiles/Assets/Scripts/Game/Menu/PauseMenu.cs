using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject pauseMenuCanvas;

    private Boolean isPaused = false;

    private Button continueButton;
    private static readonly int PAUSE = 0;
    private static readonly float PLAY = 1;

    private void Awake() {
        DeactivateMenu();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            isPaused = !isPaused;

            if (isPaused) {
                ActivateMenu();
            } else {
                DeactivateMenu();
            }
        }
    }

    void ActivateMenu() {
        Time.timeScale = PAUSE;
        //Time.fixedDeltaTime = PAUSE;
        pauseMenuCanvas.SetActive(true);
    }

    public void DeactivateMenu() {
        Time.timeScale = PLAY;
        pauseMenuCanvas.SetActive(false);
        isPaused = false;
    }
}