using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplayGroupGUIController : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI score;
    void Start() {
        if (score == null) {
            throw new MissingComponentException("Missing TextMeshPro score component");
        }
        score.text = GameManager.Instance.GameDataManager.GameScore.ToString();
    }
}