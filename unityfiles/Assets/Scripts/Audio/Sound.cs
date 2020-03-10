using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {
    
    // Name of the sound
    public string name;
    
    // The audio clip we want to play
    public AudioClip audioClip;
    
    // The volume of the sound
    [Range(0f, 1f)]
    public float volume;
    // The pitch of the sound
    [Range(0.1f, 3f)]
    public float pitch;
    
    // If we want the sound to loop or not
    public bool loop;
    
    [HideInInspector]
    public AudioSource audioSource;
}
