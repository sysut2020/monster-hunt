using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {
    
    // Name of the sound
    [SerializeField]
    private string name;
    public string Name {
        get => name;
        set => name = value;
    }
    
    [SerializeField]
    private AudioClip audioClipToPlay;
    public AudioClip AudioClip {
        get => audioClipToPlay;
        set => audioClipToPlay = value;
    }
    
    // The volume of the sound
    [Range(0f, 1f)]
    [SerializeField]
    private float volume = 1;
    public float Volume {
        get => volume;
        set => volume = value;
    }

    // The pitch of the sound
    [Range(0.1f, 3f)]
    [SerializeField]
    private float pitch = 1;
    public float Pitch {
        get => pitch;
        set => pitch = value;
    }

    // If we want the sound to loop or not
    [SerializeField]
    private bool loop;
    public bool Loop {
        get => loop;
        set => loop = value;
    }

    [SerializeField] 
    private bool mute;
    public bool Mute {
        get => mute;
        set => mute = value;
    }
}
