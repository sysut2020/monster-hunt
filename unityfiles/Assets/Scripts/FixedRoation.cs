using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fixes the roatation of the game object the script sits on
/// Will prevent flipping from happening.
/// </summary>
public class FixedRoation : MonoBehaviour {

    Quaternion initialRotation;

    void Awake() {
        this.initialRotation = this.transform.rotation;
    }

    void LateUpdate() {
        if (!this.transform.rotation.Equals(initialRotation)) {
            this.transform.rotation = initialRotation;
        }
    }
}