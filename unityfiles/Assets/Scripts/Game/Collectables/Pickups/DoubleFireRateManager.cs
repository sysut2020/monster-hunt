using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages the double fire rate powerups
/// Has to exists in the scene for the power up to work
/// </summary>
public class DoubleFireRateManager : MonoBehaviour {

    private const PICKUP_TYPE type = PICKUP_TYPE.DOUBLE_FIRE_RATE;

    [SerializeField]
    private int unitMultiplier = 2;

    [SerializeField]
    private int secEffectDuration = 10;



    private int rollingID = 0;
    private GunController usedGunController = null;
    private int activeMultipliers = 0;

    private Dictionary<int, IEnumerator> activeCoroutines = new Dictionary<int, IEnumerator>();

    // -- properties -- //

    public int RollingID { 
        get{
            rollingID += 1;
            return rollingID;    
        }
    }

    // -- cleanup vars -- //
    private float initialFireRate;


    // -- events -- //

    private void SubscribeToEvents() {
        PlayerWeaponController.WeaponChangedEvent += CallbackWeaponChangedEvent;
        PowerupCollectable.OnPowerupCollected += CallbackOnPowerupCollected;
    }

    private void UnsubscribeFromEvents() {
        PlayerWeaponController.WeaponChangedEvent -= CallbackWeaponChangedEvent;
        PowerupCollectable.OnPowerupCollected -= CallbackOnPowerupCollected;
    }

    private void CallbackWeaponChangedEvent(object _, WeaponChangedEventArgs args) {
        this.Cleanup ();
        this.usedGunController = args.NewGunController;
        this.initialFireRate = args.NewGunController.FireRate;
        this.UpdateEffectOnController ();
    }

    private void CallbackOnPowerupCollected(object _, PowerUpCollectedArgs args) {
        if (PICKUP_TYPE.DOUBLE_FIRE_RATE == args.Effect){
            StartCoroutine(Effect(this.secEffectDuration, this.rollingID));
        }
    }

    // -- private -- //

    
    private IEnumerator Effect(int waitTime, int rid){
        this.activeMultipliers += 1;
        this.UpdateEffectOnController();
        print("aa");
        yield return new WaitForSeconds(waitTime);
        print("bb");
        this.activeMultipliers -= 1;
        this.UpdateEffectOnController();
        StopActiveCorutine(rid);
    }

    private void StopActiveCorutine(int rid){
        if (activeCoroutines.ContainsKey(rid)){
            StopCoroutine(activeCoroutines[rid]);
            activeCoroutines.Remove(rid);
        }
    }


    
    private void Cleanup () {
        if (usedGunController != null){
            usedGunController.FireRate = initialFireRate;
        }
    }

    private void UpdateEffectOnController() {
        if (this.activeMultipliers < 0){
            throw new Exception("Bopdidop somthing is wrong");
        }else if (activeMultipliers > 0) {
            this.usedGunController.FireRate = initialFireRate * this.unitMultiplier* this.unitMultiplier;
        } else{
            this.usedGunController.FireRate = initialFireRate;
        }
        print(this.usedGunController.FireRate);


    }


    private void Awake() {
        this.SubscribeToEvents();
    }

    private void OnDestroy() {
        this.UnsubscribeFromEvents();
    }

}