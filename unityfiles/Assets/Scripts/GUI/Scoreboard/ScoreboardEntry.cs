using System;

/// <summary>
/// Represents a score board entry. Holds the playerName of the player with the score and the score s/he got.
/// Implements the Comparable interface to be used if a scoreboardEntry is in a list that needs to be sorted.
/// </summary>
[Serializable]
public class ScoreboardEntry : IComparable<ScoreboardEntry> {

    public string PlayerName { get; set; }

    public int Score { get; set; }

    public ScoreboardEntry() { }

    /// <summary>
    /// Creates a scoreboard entry with a name and a score
    /// </summary>
    /// <param name="playerName">name of the entry</param>
    /// <param name="score">score amount</param>
    public ScoreboardEntry(string playerName, int score) {
        PlayerName = playerName;
        Score = score;
    }

    /// <summary>
    /// Compares this score to another score.
    /// Returns 1 if the other score is lower, returns -1 if this score is lower
    /// Returns 0 if they are equal.
    /// </summary>
    /// <param name="other">other score to compare to</param>
    /// <returns>returns 1 if this is greater, -1 if this is lower, 0 if equal</returns>
    public int CompareTo(ScoreboardEntry other) {
        if (other.Score < Score) {
            return 1;
        }
        if (other.Score > Score) {
            return -1;
        }
        return 0;
    }
}