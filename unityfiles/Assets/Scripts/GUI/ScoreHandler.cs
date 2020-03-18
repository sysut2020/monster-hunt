using UnityEngine;

/// <summary>
/// Keeps track of the current total score and level score.
/// Adds score or subtracts score when needed.
/// Saves score when destroyed.
/// </summary>
public class ScoreHandler : MonoBehaviour {
    private int totalScore;
    private int levelScore = 0;
    private DataHandler dataHandler;

    [SerializeField]
    private ScoreGUI scoreGui;


    private void Awake() {
        dataHandler = gameObject.AddComponent<DataHandler>();
    }

    private void Start() {
        LoadTotalScore();
        SubscribeToEvents();
    }

    private void OnDestroy() {
        SaveScore();
        UnsubscribeFromEvents();
    }

    private void LoadTotalScore() {
        Save data = dataHandler.LoadData();
        totalScore = data.Score;
    }

    private void SaveScore() {
        Save data = dataHandler.LoadData();
        data.Score = totalScore;
        dataHandler.SaveData(data);
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
        scoreGui.UpdateScoreText();
    }

    private void CallbackLetterCollected(object __, LetterCollectedArgs _) {
        levelScore++; // when a letter is collected we add one point to the total score counter
        scoreGui.UpdateScoreText();
    }

    private void CallbackCoinCollected(object _, CoinCollectedArgs args) {
        levelScore += args.Amount;
        scoreGui.UpdateScoreText();
    }
}