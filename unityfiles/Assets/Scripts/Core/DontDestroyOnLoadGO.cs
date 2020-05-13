using UnityEngine;

/// <summary>
/// If attached on a game object, the game object will survive all scenes.
///     The first object created will survive all scenes and overwrite an existing object on the scene if it exists.
/// </summary>
public class DontDestroyOnLoadGO : MonoBehaviour {

    public static DontDestroyOnLoadGO Instance;

    private void Awake() {
        if (Instance != this && Instance != null) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}