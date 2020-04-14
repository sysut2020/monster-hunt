using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A hacky solution to change active UI elements to Scoreboard display
/// as long as the game data score is zero
/// </summary>
public class ScoreBoard : MonoBehaviour {
    [SerializeField]
    private GameObject scoreboard;

    [SerializeField]
    private GameObject inputScreen;

    [SerializeField]
    private GameObject scoreboardScreen;

    void Awake() {
        GameDataManager dataManager = GameManager.Instance.GameDataManager;

        if (dataManager.GameScore == 0) {
            this.scoreboard.SetActive(true);
            this.scoreboardScreen.SetActive(true);
            this.inputScreen.SetActive(false);
        }
    }

    void OnDestroy() {
        this.scoreboard.SetActive(false);
        this.scoreboardScreen.SetActive(false);
        this.inputScreen.SetActive(true);
    }
}