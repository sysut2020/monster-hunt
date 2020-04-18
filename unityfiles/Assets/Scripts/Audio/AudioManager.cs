using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : Singleton<AudioManager> {

    private AudioSource gameMusic;
    private List<AudioSource> soundFX;
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);

        gameMusic = gameObject.AddComponent<AudioSource>();
        
        soundFX = new List<AudioSource>();
        
        for (int i = 0; i < 5; i++) {
            soundFX.Add(gameObject.AddComponent<AudioSource>());
        }
    }
    
    public void PlaySound(Sound sound) {
        foreach (var source in soundFX) {
            if (!source.isPlaying) {
                source.playOnAwake = sound.PlayOnAwake;
                source.loop = sound.Loop;
                source.name = sound.Name;
                source.volume = sound.Volume;
                source.pitch = sound.Pitch;
                source.mute = sound.Mute;

                source.PlayOneShot(sound.AudioClip);
                break;
            }
        }
    }

    public void PlayMusic(Sound sound) {
        gameMusic.loop = sound.Loop;
        gameMusic.name = sound.Name;
        gameMusic.volume = sound.Volume;
        gameMusic.pitch = sound.Pitch;
        gameMusic.mute = sound.Mute;
        gameMusic.clip = sound.AudioClip;
        gameMusic.playOnAwake = sound.PlayOnAwake;

        gameMusic.Play(1500);

    }
}
