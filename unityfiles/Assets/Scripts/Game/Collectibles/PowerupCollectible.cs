using System;
using UnityEngine;

/// <summary>
/// Arguments for powerup collected event
/// </summary>
public class PowerUpCollectedArgs : EventArgs {
    public PICKUP_TYPE Effect { get; set; }
}

/// <summary>
/// Used to handle the power up collectible
/// </summary>
public class PowerupCollectible : Collectible {

    [SerializeField]
    private PICKUP_TYPE effectPickup;
    public static event EventHandler<PowerUpCollectedArgs> OnPowerupCollected;

    private MoveToGuiElement moveToGuiElement;

    private void Awake() {
        if (TryGetComponent(out moveToGuiElement)) {
            moveToGuiElement.FindTarget<LettersCollectedGUI>();
        }

        PowerUpCollectedArgs powerupArgs = new PowerUpCollectedArgs();
        powerupArgs.Effect = effectPickup;
        PowerupCollectible.OnPowerupCollected?.Invoke(this, powerupArgs);
    }
}