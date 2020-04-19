using System;

/// <summary>
/// Represents a score board entry. Holds the playerName of the player with the score and the score s/he got.
/// Implements the Comparable interface to be used if a scoreboardEntry is in a list that needs to be sorted.
/// </summary>
[Serializable]
public class ScoreboardEntry : IComparable<ScoreboardEntry> {
	public ScoreboardEntry() { }

	public ScoreboardEntry(string playerName, int score) {
		PlayerName = playerName;
		Score = score;
	}

	public string PlayerName { get; set; }

	public int Score { get; set; }

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