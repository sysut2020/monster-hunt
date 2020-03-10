using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private Sound[] sounds;

    private static AudioManager instance;
    
    private void Awake() {
        
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in sounds) {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;

            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }

    private void Start() {
        Play("Level 1 music");
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name +" not found!");
            return;
        }
        s.audioSource.Play();
    }
}
