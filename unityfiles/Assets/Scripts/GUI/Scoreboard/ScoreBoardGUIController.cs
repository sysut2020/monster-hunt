using System;
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

        scoreboardEntries = new List<ScoreboardEntry>() {
            new ScoreboardEntry {Name = "spiftire", Score = 2000},
            new ScoreboardEntry {Name = "dummy", Score = 12300},
            new ScoreboardEntry {Name = "dumb", Score = 69}
        };

        scoreboardEntryTransformList = new List<Transform>();
        foreach (ScoreboardEntry entry in scoreboardEntries) {
            CreateScoreboardEntryTransform(entry, entryContainer, scoreboardEntryTransformList);
        }
        
        scoreboardEntries.Sort((entry1, entry2) => entry1.Score.CompareTo(entry2.Score));
    }

    private void CreateScoreboardEntryTransform(ScoreboardEntry scoreboardEntry, Transform container,
        List<Transform> transformList) {
        var templateHeight = entryTemplate.GetComponent<RectTransform>().rect.height;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("nameTemplateText").GetComponent<TextMeshProUGUI>().text = scoreboardEntry.Name;
        entryTransform.Find("scoreTemplateText").GetComponent<TextMeshProUGUI>().text = scoreboardEntry.Score.ToString();

        transformList.Add(entryTransform);
    }
}

public class ScoreboardEntry {
    private string name;
    private int score;

    public string Name {
        get => name;
        set => name = value;
    }

    public int Score {
        get => score;
        set => score = value;
    }

    public static int CompareByScore(ScoreboardEntry entry1, ScoreboardEntry entry2) {
        return entry1.Score.CompareTo(entry2.Score);
    }
}