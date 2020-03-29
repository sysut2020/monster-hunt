using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerAudioListner : AudioListner {

    [SerializeField] 
    private Sound mainMenuMusic;
    
    [SerializeField]
    private Sound level1Music;

    [SerializeField] 
    private Sound letterGameMusic;

    private bool loopMainMenuMusic = true;
    
    private void Awake() {
        SubscribeToEvents();
        mainMenuMusic.Loop = loopMainMenuMusic;
    }

    private void SubscribeToEvents() {
        GameManager.GameStateChangeEvent += CallbackGameStateChangeEvent;
    }

    private void CallbackGameStateChangeEvent(object o, GameStateChangeEventArgs args) {
        if (args.NewState == GAME_STATE.MAIN_MENU) {
            PlayMusic(mainMenuMusic);
        }
        if (args.NewState == GAME_STATE.START_GAME) {
            PlayMusic(level1Music);
        }
        if (args.NewState == GAME_STATE.LETTER_LEVEL) {
            PlayMusic(letterGameMusic);    
        }
    }
}
