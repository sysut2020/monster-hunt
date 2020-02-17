﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is responsible for updating the GUI with how many coins that 
/// the player has collected
/// </summary>
public class CoinsCollecedGUI : MonoBehaviour {

    /// <summary>
    /// Text for visualizing numbers of letters collected and have left.
    /// </summary>
    [SerializeField]
    TMP_Text coinCounter;

    private int collectedCoins = 0;

    private void Awake() {
        if (coinCounter == null) throw new MissingComponentException("Missing text component");
        CollectableEvents.OnCoinCollected += OnNewCoin;
        SetCoinamountText();
    }

    private void SetCoinamountText() {
        this.coinCounter.SetText($"{collectedCoins}");
    }

    private void OnNewCoin(object sender, CoinCollectedArgs coin) {
        this.collectedCoins++;
        this.SetCoinamountText();
    }

    private void OnDestroy() {
        CollectableEvents.OnCoinCollected -= OnNewCoin;
    }
}