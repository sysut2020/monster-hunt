using System;
using UnityEngine;

public class GunMovement : MonoBehaviour {
    
    [SerializeField]
    private Transform roatatePoint;

    private MouseHandler mouseHandler;

    private void Start() {
        mouseHandler = gameObject.AddComponent<MouseHandler>();
    }

    void Update() {
        // Creates a vector from the position that our aim are based of, to the mouse position
        Vector3 mouseWorldPosition = mouseHandler.MouseWorldPosition(roatatePoint.gameObject);
        float angle = mouseHandler.AngleBetweenTwoPoints(roatatePoint.position, mouseWorldPosition);
        
        if (angle < 90 && angle > -90) {
            roatatePoint.eulerAngles = new Vector3(180, 0, -angle + 180);
        } else {
            roatatePoint.eulerAngles = new Vector3(0, 0, angle + 180);
        }

    }
}