using System.Collections.Generic;

/// <summary>
/// All information we want to store is kept inside here
/// Class used in conjunction with "GameDataManager" class.
/// </summary>
[System.Serializable]
public class SaveData {
    private List<ScoreboardEntry> highScores;
    private int money;

    /// <summary>
    /// List of Highscores
    /// </summary>
    /// <value>returns a list of highscores</value>
    public List<ScoreboardEntry> HighScores {
        get => highScores;
        set => highScores = value;
    }

    /// <summary>
    /// Property for saving Money
    /// </summary>
    public int Money {
        get => money;
        set => money = value;
    }
}