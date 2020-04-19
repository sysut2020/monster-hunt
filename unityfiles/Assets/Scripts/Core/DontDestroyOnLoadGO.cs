using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place on a game object To make it survive over scene changes
/// </summary>
public class DontDestroyOnLoadGO : MonoBehaviour {

    public static DontDestroyOnLoadGO Instance;

    void Awake() {
        if (Instance != this && Instance != null) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}