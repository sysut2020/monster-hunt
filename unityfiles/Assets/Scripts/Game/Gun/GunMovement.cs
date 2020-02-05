using System;
using UnityEngine;

public class GunMovement : MonoBehaviour {
    
    // The point that we want our aim to be based of
    [SerializeField] private GameObject aimDirectionPoint;

    private MouseHandler mouseHandler;

    private void Start() {
        mouseHandler = gameObject.AddComponent<MouseHandler>();
    }

    void Update() {
        // Creates a vector from the position that our aim are based of, to the mouse position
        Vector3 mouseWorldPosition = mouseHandler.MouseWorldPosition(aimDirectionPoint);
        float angle = mouseHandler.AngleBetweenTwoPoints(transform.position, mouseWorldPosition);
        
        if (angle < 0 || angle < -130) {
            // We add 180 degrees to give the gun the right position
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180f));
        }
    }
}
