using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAngle {
    // Returns an angle between two vectors in degrees
    public float GetGunAngle(Vector3 vectorA, Vector3 vectorB) {
        return Mathf.Atan2(vectorA.y - vectorB.y, vectorA.x - vectorB.x) * Mathf.Rad2Deg;
    }
}
