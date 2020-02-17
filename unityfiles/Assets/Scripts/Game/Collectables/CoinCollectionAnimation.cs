using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectionAnimation : MonoBehaviour {

    [SerializeField] public GameObject destination;
    [SerializeField] public float animationDuration;

    public Vector3 offset;

    public void Start() {
        MoveToCollectionPoint();
    }

    public void MoveToCollectionPoint() {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", destination.transform.position + offset, "time", animationDuration, iTween.EaseType.easeInSine));
    }
}
