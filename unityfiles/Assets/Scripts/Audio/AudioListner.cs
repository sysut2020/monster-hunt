using UnityEngine;

// LISTEN HERE
public abstract class AudioListner : MonoBehaviour {

    protected void PlaySound(Sound sound) {
        AudioManager.Instance.PlaySound(sound);
    }

    protected void PlayMusic(Sound sound) {
        AudioManager.Instance.PlayMusic(sound);
    }

}