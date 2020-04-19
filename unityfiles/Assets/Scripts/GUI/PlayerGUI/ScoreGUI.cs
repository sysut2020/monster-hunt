using System;
using TMPro;
using UnityEngine;

public class ScoreGUI : MonoBehaviour {
	private TextMeshProUGUI scoreText;

	// Start is called before the first frame update
	void Awake() {
		scoreText = gameObject.GetComponent<TextMeshProUGUI>();
	}

	/// <summary>
	/// Updates the score text to the total score
	/// </summary>
	public void UpdateScoreText(int score) {
		scoreText.text = score.ToString();
	}
}