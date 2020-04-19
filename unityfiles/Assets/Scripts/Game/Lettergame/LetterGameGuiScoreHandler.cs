using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Responsible for updating and seting the current accumeleted game score, and total word score 
/// on letter game GUI. The word score incrementaly increase/decrease with a 
/// count effect as words are created/destroyed.
/// </summary>
public class LetterGameGuiScoreHandler : MonoBehaviour {

	[SerializeField]
	private TMP_Text currentScoreText;

	[SerializeField]
	private TMP_Text wordScoreText;

	private int currentWordScore = 0;

	void Awake() {
		if (this.currentScoreText == null) { throw new MissingComponentException("Add current score text component"); }
		if (this.wordScoreText == null) { throw new MissingComponentException("Add word score text component"); }
		LetterLevelController.OnWordScoreUpdateEvent += UpdateWordsScore;
	}

	private void Start() {
		this.currentScoreText.SetText(GameManager.Instance.GameDataManager.GameScore.ToString());
		this.wordScoreText.SetText(this.currentWordScore.ToString());
	}

	private void OnDestroy() {
		LetterLevelController.OnWordScoreUpdateEvent -= UpdateWordsScore;
	}

	private void UpdateWordsScore(object _, WordScoreUpdateArgs args) {
		int newScore = args.Score;

		if (newScore != currentWordScore || newScore != 0) {
			StartCoroutine(UpdateScoreText(newScore - currentWordScore));
		}
	}

	/// <summary>
	/// Incrementaly increase/decrease the score value> counter effect
	/// </summary>
	/// <param name="diff"></param>
	/// <returns></returns>
	private IEnumerator UpdateScoreText(int diff) {
		bool countDown = diff < 0;
		diff = Math.Abs(diff);
		for (int i = 0; i < diff; i++) {
			yield return new WaitForSeconds(.02f);
			this.currentWordScore += countDown ? -1 : 1;
			this.wordScoreText.SetText(this.currentWordScore.ToString());
		}
	}
}