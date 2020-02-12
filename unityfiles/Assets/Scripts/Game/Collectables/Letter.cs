﻿using System;
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
        if (GameObject.Find("Letter " + letterString)) {
            CollectibleEvents.LetterCollected.Invoke(this, this);
        }
    }
}
