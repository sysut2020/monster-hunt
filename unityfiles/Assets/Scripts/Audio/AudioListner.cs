using UnityEngine;

/// <summary>
/// Listens for sounds or music to be played,
/// takes in a "Sound" object to be sent to the "audio manager"
/// </summary>
public abstract class AudioListner : MonoBehaviour {

    protected void PlaySound(Sound sound) {
        AudioManager.Instance.PlaySound(sound);
    }

    protected void PlayMusic(Sound sound) {
        AudioManager.Instance.PlayMusic(sound);
    }

}