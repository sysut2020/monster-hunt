using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorHelper {

    /// <summary>
    /// Calculates and returns the angle between two 
    /// vectors
    /// </summary>
    /// <param name="vectorA"></param>
    /// <param name="vectorB"></param>
    /// <returns>the angle between the vectors in degrees</returns>
    public static float AngleBetweenVector2D (Vector3 vectorA, Vector3 vectorB) {
        return Mathf.Atan2(vectorA.y - vectorB.y, vectorA.x - vectorB.x) * Mathf.Rad2Deg;
    }
}
