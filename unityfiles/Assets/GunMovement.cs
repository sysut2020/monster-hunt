using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GunMovement : MonoBehaviour {
    
    // The point that we want our aim to be based of
    [SerializeField] private GameObject aimDirectionPoint;
    
    void Update() {
        // Creates a vector from the position that our aim are based of, to the mouse position
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + (Vector3.forward * 10f) - aimDirectionPoint.transform.position);

        float angle = AngleBetweenTwoPoints(transform.position, mouseWorldPosition);
        Debug.Log(angle);
        if (angle < 0 || angle < -130) {
            // We add 180 degrees to give the gun the right position
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180f));
        }
    }
    
    // Returns an angle between two vectors in degrees
    private float AngleBetweenTwoPoints(Vector3 vectorA, Vector3 vectorB) {
        return Mathf.Atan2(vectorA.y - vectorB.y, vectorA.x - vectorB.x) * Mathf.Rad2Deg;
    }
}
