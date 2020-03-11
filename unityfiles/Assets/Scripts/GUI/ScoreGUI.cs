using System;
using TMPro;
using UnityEngine;

public class ScoreGUI : MonoBehaviour {
    private int totalLevelScore = 0;
    private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start() {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();

        UpdateScoreText();
        SubscribeToEvents();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
        // todo save score
    }

    /// <summary>
    /// Updates the score text to the total score
    /// </summary>
    private void UpdateScoreText() {
        scoreText.text = totalLevelScore.ToString();
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
        totalLevelScore++; // when a power up is picked up we add one point to the total score
        UpdateScoreText();
    }

    private void CallbackLetterCollected(object __, LetterCollectedArgs _) {
        totalLevelScore++; // when a letter is collected we add one point to the total score counter
        UpdateScoreText();
    }

    private void CallbackCoinCollected(object _, CoinCollectedArgs args) {
        totalLevelScore += args.Amount;
        UpdateScoreText();
    }
}