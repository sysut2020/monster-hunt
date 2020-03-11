using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardGUIController : MonoBehaviour {
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<ScoreboardEntry> scoreboardEntries;
    private List<Transform> scoreboardEntryTransformList;

    private void Awake() {
        entryContainer = transform.Find("entryContainer");
        entryTemplate = entryContainer.Find("template");

        entryTemplate.gameObject.SetActive(false);
        scoreboardEntries = GameManager.Instance.GameDataManager.HighScores;

        // scoreboardEntries = new List<ScoreboardEntry>() {
        //     new ScoreboardEntry {PlayerName = "spiftire", Score = 2000},
        //     new ScoreboardEntry {PlayerName = "dummy", Score = 12300},
        //     new ScoreboardEntry {PlayerName = "dumb", Score = 69}
        // };
        
        scoreboardEntries.Sort();
        scoreboardEntries.Reverse();

        scoreboardEntryTransformList = new List<Transform>();
        foreach (ScoreboardEntry entry in scoreboardEntries) {
            CreateScoreboardEntryTransform(entry, entryContainer, scoreboardEntryTransformList);
        }
        
        // GameManager.Instance.GameDataManager.SetHighScores(scoreboardEntries);
    }

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