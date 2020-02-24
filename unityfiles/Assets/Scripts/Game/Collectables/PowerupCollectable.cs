using System;
using UnityEngine;

/// <summary>
/// Arguments for powerup collected event
/// </summary>
public class PowerUpCollectedArgs : EventArgs {
    public IPowerUp Effect { get; set; }
}

public class PowerupCollectable : Collectable {
    public static event EventHandler<PowerUpCollectedArgs> OnPowerupCollected;

    private MoveToGuiElement moveToGuiElement;

    private IPowerUp effectPickup;
    public IPowerUp EffectPickup {
        get => effectPickup;
        set => effectPickup = value;
    }

    private void Awake() {
        if (TryGetComponent(out moveToGuiElement)) {
            moveToGuiElement.FindTarget<LettersCollectedGUI>();
        }
    }

    private void OnDestroy() {
        PowerUpCollectedArgs powerupArgs = new PowerUpCollectedArgs();
        powerupArgs.Effect = effectPickup;
        PowerupCollectable.OnPowerupCollected?.Invoke(this, powerupArgs);
    }

}