using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioListner : MonoBehaviour {

	protected void PlaySound(Sound sound) {
		AudioManager.Instance.PlaySound(sound);
	}

	protected void PlayMusic(Sound sound) {
		AudioManager.Instance.PlayMusic(sound);
	}

}