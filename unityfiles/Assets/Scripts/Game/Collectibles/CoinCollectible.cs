using System;
using UnityEngine;

/// <summary>
/// Arguments for coin collected event
/// </summary>
public class CoinCollectedArgs : EventArgs {
    public int Amount { get; set; }
}

/// <summary>
/// Used to handle the coin collectibles 
/// </summary>
public class CoinCollectible : Collectible {
    public static event EventHandler<CoinCollectedArgs> OnCoinCollected;

    private MoveToGuiElement moveToGuiElement;

    [SerializeField]
    private int coinValue = 1;

    private void Awake() {
        if (TryGetComponent(out moveToGuiElement)) {
            moveToGuiElement.FindTarget<CoinsCollectedGUI>();
        }

        this.ScoreValue = coinValue;

        var args = new CoinCollectedArgs();
        args.Amount = this.coinValue;
        OnCoinCollected?.Invoke(this, args);
    }
}