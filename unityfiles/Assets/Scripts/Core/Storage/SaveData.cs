using System.Collections.Generic;

/// <summary>
/// Simple class which is used in part with "DataHandler" class.
/// All information we want to store, can be kept in this class
/// </summary>
[System.Serializable]
public class SaveData {
    public List<ScoreboardEntry> HighScores;
    public int Money;
}