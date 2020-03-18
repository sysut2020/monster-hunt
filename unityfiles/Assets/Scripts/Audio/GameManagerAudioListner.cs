using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerAudioListner : AudioListner {

    [SerializeField]
    private Sound level1Music;
    
    private void Awake() {
        SubscribeToEvents();
    }

    private void SubscribeToEvents() {
        GameManager.GameStateChangeEvent += CallbackGameStateChangeEvent;
    }

    private void CallbackGameStateChangeEvent(object o, GameStateChangeEventArgs args) {
        Debug.Log("Event called");
        if (args.NewState == GAME_STATE.TEST_LEVEL) {
            Debug.Log("Playing music");
            PlayMusic(level1Music);
        }
    }
}
