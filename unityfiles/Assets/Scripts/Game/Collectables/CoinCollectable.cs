using System;
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

    private readonly string name = "Coin";

    public override string Name {
        get => name;
    }

    private void Awake() {
        if (TryGetComponent(out moveToGuiElement)) {
            moveToGuiElement.FindTarget<CoinsCollecedGUI>();
        }

        this.ScoreValue = coinValue; // idk mabye not
    }

    private void OnDestroy() {
        var args = new CoinCollectedArgs();
        args.Amount = this.coinValue;
        OnCoinCollected?.Invoke(this, args);
    }
}