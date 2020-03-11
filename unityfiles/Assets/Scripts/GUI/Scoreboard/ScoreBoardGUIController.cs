using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controls how the scoreboard is displayed.
/// Loads the different high scores from the saved data and displays them in a descending fashion
/// Script is highly inspired by Code Monkey. (source: https://www.youtube.com/watch?v=iAbaqGYdnyI)
/// </summary>
public class ScoreBoardGUIController : MonoBehaviour {
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<ScoreboardEntry> scoreboardEntries;
    private List<Transform> scoreboardEntryTransformList;

    private void Awake() {
        InitilizeFields();
        LoadHighScores();

        // scoreboardEntries = new List<ScoreboardEntry>() {
        //     new ScoreboardEntry {PlayerName = "spiftire", Score = 2000},
        //     new ScoreboardEntry {PlayerName = "dummy", Score = 12300},
        //     new ScoreboardEntry {PlayerName = "dumb", Score = 69}
        // };
        
        SortScoreBoardInDecendingOrder();

        scoreboardEntryTransformList = new List<Transform>();
        foreach (ScoreboardEntry entry in scoreboardEntries) {
            CreateScoreboardEntryTransform(entry, entryContainer, scoreboardEntryTransformList);
        }
        
        // GameManager.Instance.GameDataManager.SetHighScores(scoreboardEntries);
    }

    private void SortScoreBoardInDecendingOrder() {
        scoreboardEntries.Sort();
        scoreboardEntries.Reverse();
    }

    private void InitilizeFields() {
        entryContainer = transform.Find("entryContainer");
        entryTemplate = entryContainer.Find("template");
        entryTemplate.gameObject.SetActive(false);
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
        List<Transform> transformList) {
        var templateHeight = entryTemplate.GetComponent<RectTransform>().rect.height;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("nameTemplateText").GetComponent<TextMeshProUGUI>().text = scoreboardEntry.PlayerName;
        entryTransform.Find("scoreTemplateText").GetComponent<TextMeshProUGUI>().text =
            scoreboardEntry.Score.ToString();

        transformList.Add(entryTransform);
    }
}