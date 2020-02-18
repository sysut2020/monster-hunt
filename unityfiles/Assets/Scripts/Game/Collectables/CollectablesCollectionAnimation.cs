using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesCollectionAnimation : MonoBehaviour {

    // To make it work you only need two nodes:
    //    The source node
    //    The destination node
    [SerializeField] private Transform[] wayPointArray;

    //The time it will take for the collectible to complete the animation
    [SerializeField] private float time;

    private Transform pos;
    // When a collectible is spawn, it will create a path to the collection point and then follow that path.
    // When the collectible is done following the path, its sprite will get disabled
    public void Start() {
        pos= CoinsCollecedGUI.TryGetTransform();
    }
    
   

    private void FixedUpdate() {
        Vector3 e = Vector3.MoveTowards(this.transform.position, pos.position, 0.2f);
       this.transform.position = e;
    }
    }
}