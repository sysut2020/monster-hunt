using System;
using UnityEngine;

/// <summary>
/// Arguments for powerup collected event
/// </summary>
public class PowerUpCollectedArgs : EventArgs {
    public PICKUP_TYPE Effect { get; set; }
}

public class PowerupCollectable : Collectable {

    [SerializeField]
    private PICKUP_TYPE effectPickup;
    public static event EventHandler<PowerUpCollectedArgs> OnPowerupCollected;

    private MoveToGuiElement moveToGuiElement;

    private void Awake () {
        if (TryGetComponent (out moveToGuiElement)) {
            moveToGuiElement.FindTarget<LettersCollectedGUI> ();
        }
    }

    private void OnDestroy () {
        PowerUpCollectedArgs powerupArgs = new PowerUpCollectedArgs ();
        powerupArgs.Effect = effectPickup;
        PowerupCollectable.OnPowerupCollected?.Invoke (this, powerupArgs);
    }

}