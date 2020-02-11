using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {
    
    [SerializeField]
    private PlayerInventory playerInventory;

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out Coin coin)) {
            playerInventory.AddMoney(coin.CoinValue);
            Destroy(coin);
        }
        if (collision.gameObject.TryGetComponent(out Letter letter)) {
            Debug.Log("We hit the letter");
        }
        if (collision.gameObject.TryGetComponent(out IEffectPickup effectPickup)) {
            Debug.Log("We hit the PowerUP");
        }
        
    }
}
