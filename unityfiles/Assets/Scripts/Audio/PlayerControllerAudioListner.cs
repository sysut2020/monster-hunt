using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens for player events like when player has been damaged
/// </summary>
public class PlayerControllerAudioListner : AudioListner {

    [SerializeField]
    private Sound damageSound1;

    [SerializeField]
    private Sound damageSound2;

    [SerializeField]
    private Sound damageSound3;

    private List<Sound> audioList;

    private int soundIndex;

    private void Awake() {
        soundIndex = 0;
        this.audioList = new List<Sound>();
        this.audioList.Add(damageSound1);
        this.audioList.Add(damageSound2);
        this.audioList.Add(damageSound3);
        SubscribeToEvents();
    }

    private void SubscribeToEvents() {
        Player.OnPlayerDamagedEvent += CallbackDamagedEvent;
    }

    private void CallbackDamagedEvent(object o, EventArgs args) {
        PlaySound(NextSound());
    }

    private Sound NextSound() {
        Sound sound = null;
        if (soundIndex < (this.audioList.Count - 1)) {
            sound = this.audioList[soundIndex];
            soundIndex++;
            return sound;
        } else {
            sound = this.audioList[soundIndex];
            soundIndex = 0;
            return sound;
        }
        return damageSound1;
    }

}