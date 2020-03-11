using System;
using TMPro;
using UnityEngine;

public class ScoreGUI : MonoBehaviour {
    private int totalLevelScore = 0;
    private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start() {
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.text = totalLevelScore.ToString();

        SubscribeToEvents();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
        // todo save score
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
    }

    private void CallbackLetterCollected(object __, LetterCollectedArgs _) {
        totalLevelScore++; // when a letter is collected we add one point to the total score counter
    }

    private void CallbackCoinCollected(object _, CoinCollectedArgs args) {
        totalLevelScore += args.Amount;
    }

    // Update is called once per frame
    void Update() {
    }
}