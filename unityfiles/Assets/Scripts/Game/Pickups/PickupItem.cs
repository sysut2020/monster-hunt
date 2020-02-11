using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

    private PlayerInventory playerInventory;
    
    public void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.TryGetComponent(out PickupItem pickupItem)) {
            Debug.Log("We hit the desired object");
            
        }

    }
}
