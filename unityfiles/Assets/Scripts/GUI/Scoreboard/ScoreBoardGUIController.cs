using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controls how the scoreboard is displayed.
/// Loads the different high scores from the saved data and displays them in a descending fashion
/// Script is highly inspired by Code Monkey. (source: https://www.youtube.com/watch?v=iAbaqGYdnyI)
/// </summary>
public class ScoreBoardGUIController : MonoBehaviour {

	[SerializeField]
	private Transform entryContainer;

	[SerializeField]
	private ScoreBoardEntryGUI GUIScoreBoardEntry;
	private List<ScoreboardEntry> scoreboardEntries;

	[Header("Testing")]
	[SerializeField]
	private bool fillWithTestData = false;

	private void Start() {
		LoadHighScores();
		SortScoreBoardInDescendingOrder();
		if (fillWithTestData) FillWithTestData();
		var scoreboardEntryTransformList = new List<ScoreBoardEntryGUI>();
		foreach (ScoreboardEntry entry in scoreboardEntries) {
			CreateScoreboardEntryTransform(entry, entryContainer, scoreboardEntryTransformList);
		}
	}
	private void SortScoreBoardInDescendingOrder() {
		scoreboardEntries?.Sort();
		scoreboardEntries?.Reverse();
	}

	private void LoadHighScores() {
		scoreboardEntries = GameManager.Instance.GameDataManager.HighScores;
	}

	/// <summary>
	/// Creates an entry in the scoreboard.
	/// </summary>
	/// <param name="scoreboardEntry">The entry to be displayed. Uses the entry fields to show player name and score on screen</param>
	/// <param name="container">What container the score should be child of</param>
	/// <param name="transformList">The list of all the other scores</param>
	private void CreateScoreboardEntryTransform(ScoreboardEntry scoreboardEntry, Transform container,
		List<ScoreBoardEntryGUI> transformList) {

		var guiEntry = Instantiate(GUIScoreBoardEntry, container);
		guiEntry.SetEntry(scoreboardEntry);
		guiEntry.transform.SetParent(container);
		transformList.Add(guiEntry);

	}

	private void FillWithTestData() {
		scoreboardEntries = new List<ScoreboardEntry>() {
			new ScoreboardEntry { PlayerName = "spiftire", Score = 2000 },
			new ScoreboardEntry { PlayerName = "dummy", Score = 12300 },
			new ScoreboardEntry { PlayerName = "dumb", Score = 69 }
		};
	}
}