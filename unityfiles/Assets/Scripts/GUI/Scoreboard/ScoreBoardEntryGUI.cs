using TMPro;
using UnityEngine;

/// <summary>
/// Responsible for setting a ScoreBoardEntry to two text fields for displaying,
/// the score name and score value.
/// </summary>
public class ScoreBoardEntryGUI : MonoBehaviour {

    [SerializeField]
    private TMP_Text name;

    [SerializeField]
    private TMP_Text score;

    private void Awake() {
        if (name == null) {
            throw new MissingComponentException("Missing TextMeshPro name component");
        }
        if (score == null) {
            throw new MissingComponentException("Missing TextMeshPro score component");
        }
    }

    /// <summary>
    /// Set GUI name and score to the provided entry.
    /// </summary>
    /// <param name="entry">score entry to add to gui</param>
    public void SetEntry(ScoreboardEntry entry) {
        if (entry == null) { return; }
        this.name.SetText(entry.PlayerName);
        this.score.SetText(entry.Score.ToString());
    }
}
