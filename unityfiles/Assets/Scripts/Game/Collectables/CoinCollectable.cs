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

public class CoinCollectable : Collectable {
    public static event EventHandler<CoinCollectedArgs> OnCoinCollected;

    private MoveToGuiElement moveToGuiElement;

    [SerializeField]
    private int coinValue = 1;

    private void Awake() {
        if (TryGetComponent(out moveToGuiElement)) {
            moveToGuiElement.FindTarget<CoinsCollecedGUI>();
        }
    }

    private void OnDestroy() {
        var args = new CoinCollectedArgs();
        args.Amount = this.coinValue;
        OnCoinCollected?.Invoke(this, args);
    }

}