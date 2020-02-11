public class CollectableEvents {

	public delegate void OnCoinPickup(int value);
	public static event OnCoinPickup onCoinPickup;
	public static void InvokeCoinPickup(int value) {
		onCoinPickup?.Invoke(value);
	}

	public delegate void OnPowerupPickup(IEffectPickup effectPickup);
	public static event OnPowerupPickup onPowerupPickup;
	public static void InvokePowerupPickup(IEffectPickup effectPickup) {
		onPowerupPickup?.Invoke(effectPickup);
	}
	

	public delegate void OnLetterPickup(Letter letter);
	public static event OnLetterPickup onLetterPickup;
	public static void InvokeLetterPickup(Letter letter) {
		onLetterPickup?.Invoke(letter);
	}

}