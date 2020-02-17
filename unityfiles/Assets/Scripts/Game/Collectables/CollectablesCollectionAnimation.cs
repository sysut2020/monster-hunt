using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesCollectionAnimation : MonoBehaviour {
    [SerializeField] private Transform[] wayPointArray;
    [SerializeField] private float time;
    
    // When a collectible is spawn, it will create a path to the collection point and then follow that path.
    // When the collectible is done following the path, its sprite will get disabled
    public void Start() {
        iTween.MoveTo(this.gameObject, iTween.Hash("path", wayPointArray, "time", time, "easetype", iTween.EaseType.easeInOutQuad, "oncomplete", "OnAnimationComplete"));
    }
    
    private void OnAnimationComplete() {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
