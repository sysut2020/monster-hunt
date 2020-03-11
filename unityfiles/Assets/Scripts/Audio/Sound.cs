using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {
    
    // Name of the sound
    private string name;
    public string Name {
        get => name;
        set => name = value;
    }

    private AudioClip audioClipToPlay;

    public AudioClip AudioClip {
        get => audioClip;
        set => audioClip = value;
    }
    
    // The volume of the sound
    [Range(0f, 1f)]
    private float volume;
    public float Volume {
        get => volume;
        set => volume = value;
    }

    // The pitch of the sound
    [Range(0.1f, 3f)]
    private float pitch;
    public float Pitch {
        get => pitch;
        set => pitch = value;
    }

    // If we want the sound to loop or not
    private bool loop;
    public bool Loop {
        get => loop;
        set => loop = value;
    }
    
    [HideInInspector]
    public AudioSource audioSource;
}
