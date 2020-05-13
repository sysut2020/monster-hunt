using TMPro;
using UnityEngine;

/// <summary>
/// Automatically updates the score text to the latest value from the data manager.
/// </summary>
public class ScoreDisplayGroupGUIController : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI score;

    private void Start() {
        if (score == null) {
            throw new MissingComponentException("Missing TextMeshPro score component");
        }
        score.text = GameManager.Instance.GameDataManager.GameScore.ToString();
    }
}