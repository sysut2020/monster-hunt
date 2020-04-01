using System;
using TMPro;
using UnityEngine;

public class ScoreGUI : MonoBehaviour {
    private int totalLevelScore = 0;
    private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Awake() {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Updates the score text to the total score
    /// </summary>
    public void UpdateScoreText(int score) {
        totalLevelScore = score;
        scoreText.text = totalLevelScore.ToString();
    }
}