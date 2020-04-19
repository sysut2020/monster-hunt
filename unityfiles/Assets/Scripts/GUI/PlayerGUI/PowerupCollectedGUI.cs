using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for updating the GUI with the powerup the player
/// has picked up and reset when effect is over.
/// </summary>
public class PowerupCollectedGUI : MonoBehaviour {
	private TMP_Text powerupName;
	private Image powerupImage;

	[SerializeField]
	[Tooltip("The default sprite to use for powerups")]
	private Sprite powerup;

	[SerializeField]
	[Tooltip("Link the sprite image to use with double fire rate")]
	private Sprite doubleFirerate;

	// Start is called before the first frame update
	void Start() {
		this.powerupName = GetComponentInChildren<TMP_Text>();
		this.powerupImage = GetComponentInChildren<Image>();

		if (this.powerupName == null) {
			throw new MissingComponentException("Missing text component");
		}
		if (this.powerupImage == null) {
			throw new MissingComponentException("Missing image component");
		}
		if (this.powerup == null) {
			throw new MissingComponentException("Missing default sprite component");
		}
		if (this.doubleFirerate == null) {
			throw new MissingComponentException("Missing double fire rate sprite component");
		}
		this.SubscribeToEvents();
		this.powerupName.text = "";
	}

	private void CallbackOnDoubleFireRateStateChange(object sender, OnPickupDoubleFireRateArgs args) {
		if (args.Active) {
			this.SetPowerupName("Double fire rate");
			this.SetPowerupImage(this.doubleFirerate);
		} else {
			this.ResetPowerup();
		}
	}

	private void SubscribeToEvents() {
		DoubleFireRateManager.OnDoubleFireRateStateChange += CallbackOnDoubleFireRateStateChange;
	}

	private void UnsubscribeFromEvents() {
		DoubleFireRateManager.OnDoubleFireRateStateChange -= CallbackOnDoubleFireRateStateChange;
	}

	private void SetPowerupName(string type) {
		this.powerupName.SetText($"{type}");
	}

	private void SetPowerupImage(Sprite image) {
		this.powerupImage.sprite = image;
	}

	private void ResetPowerup() {
		this.SetPowerupName("");
		this.SetPowerupImage(this.powerup);
	}

	// triggers when mono behaviour object is destroyed
	private void OnDestroy() {
		this.UnsubscribeFromEvents();
	}

}