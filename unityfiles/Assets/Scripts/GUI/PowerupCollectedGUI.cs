using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for updating the GUI with the powerup the player
/// has picked up and reset when effect is over.
/// </summary>
public class PowerupCollectedGUI : MonoBehaviour {

    [SerializeField]
    [Tooltip("The game object to hold the Power Up text")]
    private TMP_Text powerupType;

    [SerializeField]
    [Tooltip("Game object to hold the Power Up image")]
    private Image powerupImage;

    [SerializeField]
    [Tooltip("The default sprite to use for powerups")]
    private Sprite powerup;

    [SerializeField]
    [Tooltip("Link the sprite image to use with double fire rate")]
    private Sprite doubleFirerate;

    // Start is called before the first frame update
    void Start() {
        if (powerupType == null) {
            throw new MissingComponentException("Missing text component");
        }
        if (powerupImage == null) {
            throw new MissingComponentException("Missing image component");
        }
        if (powerup == null) {
            throw new MissingComponentException("Missing default sprite component");
        }
        if (doubleFirerate == null) {
            throw new MissingComponentException("Missing double fire rate sprite component");
        }
        this.SubscribeToEvents();
        this.powerupType.text = "";
    }

    private void CallbackOnActiveDoubleFireRatePowerup(object sender, OnPickupDoubleFireRateArgs powerup) {
        this.SetPowerupType("Double fire rate");
        this.SetPowerupImage(this.doubleFirerate);
    }
    private void CallbackOnExpiredDoubleFireRatePowerup(object sender, OnPickupDoubleFireRateArgs powerup) {
        this.SetPowerupType("...");
        this.SetPowerupImage(this.powerup);
    }

    private void SubscribeToEvents() {
        DoubleFireRateManager.OnPickupDoubleFireRateActive += CallbackOnActiveDoubleFireRatePowerup;
        DoubleFireRateManager.OnPickupDoubleFireRateExpired += CallbackOnExpiredDoubleFireRatePowerup;
    }

    private void UnsubscribeToEvents() {
        DoubleFireRateManager.OnPickupDoubleFireRateActive -= CallbackOnActiveDoubleFireRatePowerup;
        DoubleFireRateManager.OnPickupDoubleFireRateExpired -= CallbackOnExpiredDoubleFireRatePowerup;
    }

    private void SetPowerupType(string type) {
        this.powerupType.SetText($"{type}");
    }

    private void SetPowerupImage(Sprite image) {
        this.powerupImage.sprite = image;
    }

    // triggers when mono behaviour object is destroyed
    private void OnDestroy() {
        this.UnsubscribeToEvents();
    }

}