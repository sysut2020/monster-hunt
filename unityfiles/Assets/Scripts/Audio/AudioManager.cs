﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
    
    public Sound[] sounds;
    
    private void Awake() {
        // Add all the sounds to our audio manager
        foreach (Sound s in sounds) {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;

            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }
    /// <summary>
    /// Plays the audio clip with a given name,
    /// returns a warning if not found.
    /// </summary>
    /// <param name="name">The audio clip we want to play</param>
    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name +" not found!");
            return;
        }
        s.audioSource.Play();
    }
}