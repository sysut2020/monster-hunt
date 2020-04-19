using UnityEngine;

/// <summary>
/// Responsible for starting the level music on the level it is attached on
/// </summary>
public class LevelMusicStarter : MonoBehaviour {

    [SerializeField]
    private AudioClip levelMusic;

    [SerializeField]
    private bool playOnAwake = true;

    [SerializeField]
    private bool loop = true;

    private void Start() {
        Sound levelSound = new Sound();
        levelSound.AudioClip = this.levelMusic;
        levelSound.Loop = this.loop;
        levelSound.PlayOnAwake = this.playOnAwake;
        levelSound.Name = this.levelMusic.name;
        AudioManager.Instance.PlayMusic(levelSound);
    }

}