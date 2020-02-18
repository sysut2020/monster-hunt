using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arguments for coin colected event
/// </summary>
public class CoinCollectedArgs : EventArgs {
    public int Amount { get; set; }
}

public class Coin : MonoBehaviour {
    public static event EventHandler<CoinCollectedArgs> OnCoinCollected;
    private int coinValue = 1;
    public int CoinValue {
        get => coinValue;
        set => coinValue = value;
    }
    private void OnDestroy() {
        var args = new CoinCollectedArgs();
        args.Amount = this.coinValue;
        this.TriggerCoinCollected(args);
    }

    private void TriggerCoinCollected(CoinCollectedArgs args) {
        OnCoinCollected?.Invoke(this, args);
    }
}