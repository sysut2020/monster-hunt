using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControllerAudioListner : AudioListner {
    
    [SerializeField]
    private Sound sniperFireSound;

    [SerializeField] 
    private Sound laserFireSound;

    private Sound fireSoundToPlay;
    
    private void Awake() {
        SubscribeToEvents();
        fireSoundToPlay = sniperFireSound;
    }

    private void SubscribeToEvents() {
        GunController.BulletFireEvent += CallbackBulletFireEvent;
        PlayerWeaponController.WeaponChangedEvent += CallbackWeaponChangeEvent;
    }

    private void CallbackBulletFireEvent(object o, EventArgs args) {
        PlaySound(fireSoundToPlay);
    }

    private void CallbackWeaponChangeEvent(object o, WeaponChangedEventArgs args) {
        if (args.GunIndex == 0) {
            fireSoundToPlay = sniperFireSound;
        }
        if (args.GunIndex == 1) {
            fireSoundToPlay = laserFireSound;
        }
    }
}
