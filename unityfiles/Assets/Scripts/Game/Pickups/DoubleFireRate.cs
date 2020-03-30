using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFireRate : MonoBehaviour, IWeaponEffectPowerUp {

    private string pickupName = "doubleFireRate";

    [SerializeField]
    private int unitMultiplier = 2;

    [SerializeField]
    private int millisecEffectDuration = 2;

    // -- internal -- // 
    private int rollingNameAppend;
    private GunController usedGunController;
    private WUTimers effectTimer = new WUTimers ();
    private Dictionary<string, int> activeMultipliers = new Dictionary<string, int> ();

    // -- properties -- //

    // -- cleanup vars -- //
    private float initialFireRate;

    // -- public -- // 

    public string GetPickupName () {
        return pickupName;
    }

    public void ApplyEffect (Player player) {
        this.usedGunController = player.PlayerWeaponController.ActiveGunController;
        initialFireRate = this.usedGunController.FireRate;
        this.ExtendEffect (this);
    }

    public void ExtendEffect (IEffectPowerUp extender) {
        if (extender is DoubleFireRate) {
            string timerId = $"fireRate_{this.rollingNameAppend}";
            DoubleFireRate ex = extender as DoubleFireRate;
            effectTimer.Set (timerId, millisecEffectDuration);
            activeMultipliers.Add (timerId, ex.unitMultiplier);
            this.rollingNameAppend ++;
        }
    }

    public void OnChangeWeapon (GunController newGunC) {
        this.Cleanup ();
        this.usedGunController = newGunC;
        this.AddEffectToController ();
    }

    public bool IsEffectFinished () {
        if (activeMultipliers.Count == 0) {
            return true;
        }
        return false;
    }

    public void Cleanup () {
        usedGunController.FireRate = initialFireRate;
    }

    // -- private -- //

    private void AddEffectToController () {
        int finalMulti = 0;
        foreach (int multi in this.activeMultipliers.Values) {
            finalMulti += multi;
        }

        if (finalMulti != 0) {
            this.usedGunController.FireRate = initialFireRate * this.unitMultiplier;
        }

    }

}