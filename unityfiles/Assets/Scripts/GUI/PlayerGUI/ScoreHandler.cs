using UnityEngine;

/// <summary>
/// Keeps track of the current level score.
/// Adds score score when needed.
/// Saves score for transfer when destroyed.
/// </summary>
public class ScoreHandler : MonoBehaviour {

    /// <summary>
    /// Current level score
    /// </summary>
    private int levelScore = 0;

    private GameDataManager dataManager;

    [SerializeField]
    private ScoreGUI scoreGui;

    private void Start() {
        dataManager = GameManager.Instance.GameDataManager;
        SubscribeToEvents();
        UpdateScore(0);
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    /// <summary>
    /// Saves data to be retrieved when switching scenes
    /// </summary>
    private void SaveScore() {
        dataManager.AddGameScore(levelScore);
    }

    private void SubscribeToEvents() {
        CoinCollectible.OnCoinCollected += CallbackCoinCollected;
        LetterCollectible.OnLetterCollected += CallbackLetterCollected;
        PowerupCollectible.OnPowerupCollected += CallbackEffectPickup;
        HuntingLevelController.OnLevelStateChangeEvent += CallbackLevelStateChange;
    }

    private void CallbackLevelStateChange(object _, LevelStateChangeEventArgs e) {
        if (e.NewState == LEVEL_STATE.GAME_WON) {
            SaveScore();
        }
    }

    private void UnsubscribeFromEvents() {
        CoinCollectible.OnCoinCollected -= CallbackCoinCollected;
        LetterCollectible.OnLetterCollected -= CallbackLetterCollected;
        PowerupCollectible.OnPowerupCollected -= CallbackEffectPickup;
        HuntingLevelController.OnLevelStateChangeEvent -= CallbackLevelStateChange;
    }

    /// <summary>
    /// Updates score text with game score + new levelscore
    /// </summary>
    /// <param name="score">The new score to be added to the total</param>
    private void UpdateScore(int score) {
        scoreGui.UpdateScoreText(dataManager.GameScore + score);
        SaveScore();
    }

    /// <summary>
    /// Adds the collectibles score value to the total score
    /// </summary>
    /// <param name="col">Sender object</param>
    /// <param name="args">event args</param>
    private void CallbackEffectPickup(object col, PowerUpCollectedArgs e) {
        this.levelScore++;
        UpdateScore(this.levelScore);
    }

    /// <summary>
    /// Adds the collectibles score value to the total score
    /// </summary>
    /// <param name="col">Sender object</param>
    /// <param name="args">event args</param>
    private void CallbackLetterCollected(object col, LetterCollectedArgs _) {
        this.levelScore += ((Collectible) col).ScoreValue;
        UpdateScore(this.levelScore);
    }

    /// <summary>
    /// Adds the collectibles score value to the total score
    /// </summary>
    /// <param name="col">Sender object</param>
    /// <param name="_">event args</param>
    private void CallbackCoinCollected(object col, CoinCollectedArgs _) {
        this.levelScore += ((Collectible) col).ScoreValue;
        UpdateScore(this.levelScore);
    }
}