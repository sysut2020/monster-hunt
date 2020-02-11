using System;

/// <summary>
/// Collectable events for triggering collection of different collecables 
/// </summary>
public class CollectableEvents {

	// public static EventHandler<Coin> CoinCollected;
	// public void OnCoinCollected(object sender, Coin coin) {
	// 	LetterCollected?.Invoke(sender, coin);
	// }

	// public static EventHandler<PowerupClass> PowerupCollected;
	// public void OnPowerupCollected(object sender, PowerupClass powerup) {
	// 	LetterCollected?.Invoke(sender, powerup);
	// }

	public static EventHandler<LetterController> LetterCollected;
	public void OnLetterCollected(object sender, LetterController letterController) {
		LetterCollected?.Invoke(sender, letterController);
	}

}