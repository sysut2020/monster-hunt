using System;
using UnityEngine;

/// <summary>
/// Listens for different gun sounds to be played
/// </summary>
public class GunControllerAudioListner : AudioListner {

    [SerializeField]
    private Sound sniperFireSound;

    [SerializeField]
    private Sound laserFireSound;

    [SerializeField]
    private Sound poopFireSound;

    private Sound fireSoundToPlay;

    private int sniperIndex = 0;
    private int laserIndex = 1;
    private int poopIndex = 2;

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
        if (args.GunIndex == sniperIndex) {
            fireSoundToPlay = sniperFireSound;
        }
        if (args.GunIndex == laserIndex) {
            fireSoundToPlay = laserFireSound;
        }

        if (args.GunIndex == poopIndex) {
            fireSoundToPlay = poopFireSound;
        }
    }
}