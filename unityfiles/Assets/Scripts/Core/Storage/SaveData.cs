using System.Collections.Generic;

/// <summary>
/// Simple class which is used in part with "GameDataManager" class.
/// All information we want to store, can be kept in this class
/// </summary>
[System.Serializable]
public class SaveData {
    private List<ScoreboardEntry> highScores;
    private int money;

    public List<ScoreboardEntry> HighScores {
        get => highScores;
        set => highScores = value;
    }

    public int Money {
        get => money;
        set => money = value;
    }
}