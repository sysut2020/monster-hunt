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
    }

    private void CreateScoreboardEntryTransform(ScoreboardEntry scoreboardEntry, Transform container, List<Transform> transformList) {
        var templateHeight = entryTemplate.GetComponent<RectTransform>().rect.height;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        Debug.Log(entryRectTransform.anchoredPosition);
        entryTransform.gameObject.SetActive(true);

        var name = scoreboardEntry.Name;
        var score = scoreboardEntry.Score;

        entryTransform.Find("nameTemplate").GetComponent<TextMeshPro>().text = name;
        entryTransform.Find("scoreTemplate").GetComponent<TextMeshPro>().text = score.ToString();
        
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
}