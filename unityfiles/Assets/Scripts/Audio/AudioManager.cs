using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEventArgs : EventArgs {
}

public class AudioManager : Singleton<AudioManager> {
    
    public Sound[] sounds;
    
    private void Awake() {
        SubscribeToEvents();
        
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in sounds) {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.AudioClip;
            
            // So that we dont have to manually write the name of the songs
            s.Name = s.audioSource.clip.name;

            s.audioSource.volume = s.Volume;
            s.audioSource.pitch = s.Pitch;
            s.audioSource.loop = s.Loop;
            s.audioSource.mute = s.Mute;
        }
    }

    private void SubscribeToEvents() {
        GameManager.OnMainMenuMusic += CallBackMainMenuMusicEvent;
        GameManager.OnLevel1Music += CallBackLevel1MusicEvent;
        Debug.Log("Handlers added");
    }

    private void UnsubscribeFromEvents() {
        GameManager.OnMainMenuMusic -= CallBackMainMenuMusicEvent;
        GameManager.OnLevel1Music -= CallBackLevel1MusicEvent;
    }
    
    private void CallBackMainMenuMusicEvent(object o, EventArgs args) {
        PlaySound("Main menu music");
    }

    private void CallBackLevel1MusicEvent(object o, EventArgs args) {
       PlaySound("Level 1 music");
       Debug.Log("Play the sound");
    }

    private void PlaySound(string name) {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.audioSource.Play();
    }
}
