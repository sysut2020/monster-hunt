using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour {

    private int timeLeft;
    private int score;

    [SerializeField]
    private TextMeshProUGUI timeLeftText;

    [SerializeField]
    private Button continueButton;
    private void Awake() {
        if (continueButton.IsInteractable()) {
            continueButton.interactable = false;
        }
        
        Debug.Log(timeLeftText.text);
        score = 0;
        timeLeftText.text = timeLeft.ToString();
    }

    private void OnEnable() {
        timeLeft = 1000; // todo get time from game
    }

    private void Update() {
        if (timeLeft > 0) {
            // todo add time to score.
            int scoreLog = (int) Math.Ceiling(Math.Log(score+2)*2); // todo use better function for this
            score += scoreLog;
            timeLeft-= scoreLog;
            timeLeftText.text = timeLeft.ToString();
        }

        if (timeLeft < 0) {
            timeLeftText.text = 0.ToString(); // setting string to 0
        }

        if (timeLeftText.text == 0.ToString()) {
            continueButton.interactable = true;
        }
    }
}