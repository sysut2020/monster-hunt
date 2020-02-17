using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour {
    private string letterString;
    public string LetterString {
        get => letterString;
        set => letterString = value;
    }

    private void Start() {
        // If a Letter exists, it will tell the collectible events handler, that it exists
        if (GameObject.Find("Letter " + letterString)) {
            CollectibleEvents.LetterCollected.Invoke(this, this);
        }
    }
}
