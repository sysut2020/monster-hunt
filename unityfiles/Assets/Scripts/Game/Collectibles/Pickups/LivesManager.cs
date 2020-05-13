using System;
using UnityEngine;

public class OnPickupLivesUpdateArgs : EventArgs {
    public int LivesToAdd { get; set; }
}

/// <summary>
/// Manages the lives pickups and adds the lives to the player
/// Has to exists in the scene for the power up to work
/// </summary>
public class LivesManager : MonoBehaviour {

    [SerializeField]
    private int livesValue = 2;
    public static event EventHandler<OnPickupLivesUpdateArgs> OnPickupLivesUpdate;

    private void SubscribeToEvents() {
        PowerupCollectible.OnPowerupCollected += CallbackOnPowerupCollected;
    }

    private void UnsubscribeFromEvents() {
        PowerupCollectible.OnPowerupCollected -= CallbackOnPowerupCollected;
    }

    private void CallbackOnPowerupCollected(object _, PowerUpCollectedArgs args) {
        if (PICKUP_TYPE.LIVE == args.Effect) {
            var argsss = new OnPickupLivesUpdateArgs();
            argsss.LivesToAdd = livesValue;
            OnPickupLivesUpdate?.Invoke(this, argsss);
        }
    }

    private void Awake() {
        this.SubscribeToEvents();
    }

    private void OnDestroy() {
        this.UnsubscribeFromEvents();
    }
}