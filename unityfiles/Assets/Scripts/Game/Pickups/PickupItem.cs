using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {
    
    [SerializeField]
    private PlayerInventory playerInventory;

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out Coin coin)) {
            Debug.Log("We hit the coin");
        }
        if (collision.gameObject.TryGetComponent(out Letter letter)) {
            Debug.Log("We hit the letter");
        }
        if (collision.gameObject.TryGetComponent(out PowerUP powerUp)) {
            Debug.Log("We hit the PowerUP");
        }
        
    }
}
