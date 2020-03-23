using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControllerAudioListner : AudioListner {
    
    [SerializeField]
    private Sound gunFireSound;
    
    private void Awake() {
        SubscribeToEvents();
    }

    private void SubscribeToEvents() {
        GunController.BulletFireEvent += CallbackBulletFireEvent;
    }

    private void CallbackBulletFireEvent(object o, EventArgs args) {
        PlaySound(gunFireSound);
    }
}
