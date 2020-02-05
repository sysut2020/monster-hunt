using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour {
    // Returns a vector from the position that our aim are based of, to the mouse position
    public Vector3 MouseWorldPosition(GameObject aimDirectionPoint) {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition + (Vector3.forward * 10f) -
                                              aimDirectionPoint.transform.position);
    }
    
    // Returns an angle between two vectors in degrees
    public float AngleBetweenTwoPoints(Vector3 vectorA, Vector3 vectorB) {
        return Mathf.Atan2(vectorA.y - vectorB.y, vectorA.x - vectorB.x) * Mathf.Rad2Deg;
    }
}
