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
    }

    private void UnsubscribeFromEvents() {
        CoinCollectable.OnCoinCollected -= CallbackCoinCollected;
        LetterCollectable.OnLetterCollected -= CallbackLetterCollected;
        PowerupCollectable.OnPowerupCollected -= CallbackEffectPickup;
    }

    private void CallbackEffectPickup(object sender, PowerUpCollectedArgs e) {
        levelScore++; // when a power up is picked up we add one point to the total score
        scoreGui.UpdateScoreText(this.levelScore);
    }

    private void CallbackLetterCollected(object __, LetterCollectedArgs _) {
        levelScore++; // when a letter is collected we add one point to the total score counter
        scoreGui.UpdateScoreText(this.levelScore);
    }

    private void CallbackCoinCollected(object _, CoinCollectedArgs args) {
        levelScore += args.Amount;
        scoreGui.UpdateScoreText(this.levelScore);
    }
}