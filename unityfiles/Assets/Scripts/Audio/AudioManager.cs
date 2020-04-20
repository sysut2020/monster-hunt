using System.Collections.Generic;
using UnityEngine;

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
                source.pitch = sound.Pitch;
                source.mute = sound.Mute;
                source.PlayOneShot(sound.AudioClip);
                break;
            }
        }
    }

    public void PlayMusic(Sound sound) {
        gameMusic.playOnAwake = sound.PlayOnAwake;
        gameMusic.clip = sound.AudioClip;
        gameMusic.loop = sound.Loop;
        gameMusic.name = sound.Name;
        gameMusic.Play(1500);

    }
}