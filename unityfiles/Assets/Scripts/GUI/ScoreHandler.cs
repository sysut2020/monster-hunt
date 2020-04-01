using System;
using UnityEngine;


/// <summary>
/// Keeps track of the current level score.
/// Adds score score when needed.
/// Saves score for transfer when destroyed.
/// </summary>
public class ScoreHandler : MonoBehaviour {
    private int levelScore = 0;
    private GameDataManager dataManager;

    [SerializeField]
    private ScoreGUI scoreGui;

    private void Start() {
        dataManager = GameManager.Instance.GameDataManager;
        SubscribeToEvents();
        scoreGui.UpdateScoreText(dataManager.GameScore);
    }

    private void OnDestroy() {
        SaveScore();
        UnsubscribeFromEvents();
    }

    /// <summary>
    /// Saves data to be retrieved when switching scenes
    /// </summary>
    private void SaveScore() {
        dataManager.AddGameScore(levelScore);
    }

    private void SubscribeToEvents() {
        CoinCollectable.OnCoinCollected += CallbackCoinCollected;
        LetterCollectable.OnLetterCollected += CallbackLetterCollected;
        PowerupCollectable.OnPowerupCollected += CallbackEffectPickup;
        LevelManager.OnLevelStateChangeEvent += CallbackLevelStateChange;
    }

    private void CallbackLevelStateChange(object _, LevelStateChangeEventArgs e) {
        if (e.NewState == LEVEL_STATE.GAME_WON) {
            SaveScore();
        }
    }

    private void UnsubscribeFromEvents() {
        CoinCollectable.OnCoinCollected -= CallbackCoinCollected;
        LetterCollectable.OnLetterCollected -= CallbackLetterCollected;
        PowerupCollectable.OnPowerupCollected -= CallbackEffectPickup;
        LevelManager.OnLevelStateChangeEvent -= CallbackLevelStateChange;
    }

    /// <summary>
    /// Adds the collectibles score value to the total score
    /// </summary>
    /// <param name="col">Sender object</param>
    /// <param name="args">event args</param>
    private void CallbackEffectPickup(object col, PowerUpCollectedArgs e) {
        this.levelScore++; // when a power up is picked up we add one point to the total score
        scoreGui.UpdateScoreText(this.levelScore);
    }

    /// <summary>
    /// Adds the collectibles score value to the total score
    /// </summary>
    /// <param name="col">Sender object</param>
    /// <param name="args">event args</param>
    private void CallbackLetterCollected(object col, LetterCollectedArgs _) {
        // when a letter is collected we add one point to the total score counter
        this.levelScore += ((Collectable) col).ScoreValue;
        scoreGui.UpdateScoreText(this.levelScore);
    }

    /// <summary>
    /// Adds the collectibles score value to the total score
    /// </summary>
    /// <param name="col">Sender object</param>
    /// <param name="_">event args</param>
    private void CallbackCoinCollected(object col, CoinCollectedArgs _) {
        this.levelScore += ((Collectable) col).ScoreValue;
        scoreGui.UpdateScoreText(this.levelScore);
    }
}