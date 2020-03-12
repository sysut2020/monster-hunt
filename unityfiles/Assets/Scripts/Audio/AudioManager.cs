using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEventArgs : EventArgs {
}

public class AudioManager : Singleton<AudioManager> {
    private AudioSource audioSource;
    
    private AudioClip mainMenuMusic;
    private AudioClip level1Music;
    
    
    public EventHandler<PlaySoundEventArgs> PlaySoundEvent;

    private void Awake() {
        SubscribeToEvents();
        LoadAllAudioClips();
    }

    private void SubscribeToEvents() {
        GameManager.OnMainMenuMusic += CallBackMainMenuMusicEvent;
        GameManager.OnLevel1Music += CallBackLevel1MusicEvent;
    }

    private void UnsubscribeFromEvents() {
        GameManager.OnMainMenuMusic -= CallBackMainMenuMusicEvent;
        GameManager.OnLevel1Music -= CallBackLevel1MusicEvent;
    }
    
    private void CallBackMainMenuMusicEvent(object o, EventArgs args) {
        audioSource.PlayOneShot(mainMenuMusic);
    }

    private void CallBackLevel1MusicEvent(object o, EventArgs args) {
        audioSource.PlayOneShot(level1Music);
    }

    private void LoadAllAudioClips() {
        mainMenuMusic = (AudioClip) Resources.Load("Audio/Game music/Main menu music.mp3");
        level1Music = (AudioClip) Resources.Load("Audio/Game music/Level 1 music.mp3");
    }
}
