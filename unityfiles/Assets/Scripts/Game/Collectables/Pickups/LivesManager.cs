using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnPickupLivesUpdateArgs : EventArgs {
	public int LivesToAdd { get; set; }
}


public class LivesManager : MonoBehaviour{

    [SerializeField]
    private int livesValue = 2;
    public static event EventHandler<OnPickupLivesUpdateArgs> OnPickupLivesUpdate;

    // -- events -- //
    private void SubscribeToEvents() {
        PowerupCollectable.OnPowerupCollected += CallbackOnPowerupCollected;
    }

    private void UnsubscribeFromEvents() {
        PowerupCollectable.OnPowerupCollected -= CallbackOnPowerupCollected;
    }

    private void CallbackOnPowerupCollected(object _, PowerUpCollectedArgs args) {
        if (PICKUP_TYPE.LIVE == args.Effect){
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
