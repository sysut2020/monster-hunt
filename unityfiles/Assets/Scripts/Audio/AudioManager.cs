using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
    
    [SerializeField]
    private Sound[] sounds;

    private void Awake() {
        // Add all the sounds to our audio manager
        foreach (Sound s in sounds) {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.AudioClip;

            s.audioSource.volume = s.Volume;
            s.audioSource.pitch = s.Pitch;
            s.audioSource.loop = s.Loop;
        }
    }
    /// <summary>
    /// Plays the audio clip with a given name,
    /// returns a warning if not found.
    /// </summary>
    /// <param name="name">The audio clip we want to play</param>
    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name +" not found!");
            return;
        }
        s.audioSource.Play();
    }
}
