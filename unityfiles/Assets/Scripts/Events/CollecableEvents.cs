using System;

public class CollectibleEvents {
	// When a Coin exists, update the player inventory 
	public static EventHandler<Coin> CoinCollected;
	public void OnCoinCollected(object sender, Coin coin) {
		CoinCollected?.Invoke(sender, coin);
	}
	
	// When a Letter exists, update the player inventory 
	public static EventHandler<Letter> LetterCollected;
	public void OnLetterCollected(object sender, Letter letter) {
		LetterCollected?.Invoke(sender, letter);
	}
}