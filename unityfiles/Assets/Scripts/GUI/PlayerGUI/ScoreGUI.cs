using TMPro;
using UnityEngine;

/// <summary>
/// Responsible for displayung current game score
/// </summary>
public class ScoreGUI : MonoBehaviour {

    private TextMeshProUGUI scoreText;

    void Awake() {
        if (this.TryGetComponent(out TextMeshProUGUI tmp)) {
            scoreText = tmp;
        } else {
            throw new MissingComponentException("Missing text mesh pro component.");
        }
    }

    /// <summary>
    /// Updates the score text to the total score
    /// </summary>
    public void UpdateScoreText(int score) {
        this.scoreText.text = score.ToString();
    }
}