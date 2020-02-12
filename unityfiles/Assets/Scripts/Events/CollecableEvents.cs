using System;

public class CollectibleEvents {
	public static EventHandler<Coin> CoinCollected;
	public void OnCoinCollected(object sender, Coin coin) {
		CoinCollected?.Invoke(sender, coin);
	}

	public static EventHandler<Letter> LetterCollected;

	public void OnLetterCollected(object sender, Letter letter) {
		LetterCollected?.Invoke(sender, letter);
	}
}