public class CollectableEvents {

	// public delegate void OnCoinPickup(Coin coin);
	// public static event OnCoinPickup onCoinPickup;

	// public delegate void OnPowerupPickup(Powerup val);
	// public static event OnPowerupPickup onPowerupPickup;

	public delegate void OnLetterPickup(LetterController letterController);
	public static event OnLetterPickup onLetterPickup;

}