using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    private int coinValue = 1;
    public int CoinValue {
        get => coinValue;
        set => coinValue = value;
    }

    private void Start() {
        // If a coin exists, it will tell the collectible events handler,
        // that it exist
        if (GameObject.Find("Coin")) {
            CollectibleEvents.CoinCollected.Invoke(this, this);
        }
    }
}
