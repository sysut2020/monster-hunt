using System;

/// <summary>
/// Collectable events for triggering collection of different collecables 
/// </summary>
public class CollectableEvents {

	public static EventHandler<Coin> OnCoinCollected;

	public static EventHandler<PowerUP> OnPowerupCollected;

	public static EventHandler<LetterController> OnLetterCollected;

}