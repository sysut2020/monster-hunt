using System;

/// <summary>
/// Collectable events for triggering collection of different collecables 
/// </summary>
public class CollectableEvents {

	public static EventHandler<PowerUpCollectedArgs> OnPowerupCollected;

}

/// <summary>
/// Arguments for powerup collected event
/// </summary>
public class PowerUpCollectedArgs : EventArgs {
	public IEffectPickup Effect { get; set; }
}