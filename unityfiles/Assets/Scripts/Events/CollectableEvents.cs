using System;

/// <summary>
/// Collectable events for triggering collection of different collecables 
/// </summary>
public class CollectableEvents {

	public static EventHandler<PowerUpCollectedArgs> OnPowerupCollected;

	public static EventHandler<LetterCollectedArgs> OnLetterCollected;

}

/// <summary>
/// Arguments for powerup collected event
/// </summary>
public class PowerUpCollectedArgs : EventArgs {
	public IEffectPickup Effect { get; set; }
}
/// <summary>
/// Arguments for letter collected event
/// </summary>
public class LetterCollectedArgs : EventArgs {
	public string Letter { get; set; }
}