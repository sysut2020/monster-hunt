using System;
using UnityEngine;

public class GunMovement : MonoBehaviour {
    
    [SerializeField]
    private Transform roatatePoint;

    private MousePosition mousePosition;
    private GunAngle gunAngle;

    private void Start() {
        mousePosition = new MousePosition();
        gunAngle = new GunAngle();
    }

    void Update() {
        // Creates a vector from the position that our aim are based of, to the mouse position
        Vector3 mouseWorldPosition = mousePosition.MouseWorldPosition(roatatePoint.gameObject);
        float angle = gunAngle.GetGunAngle(roatatePoint.position, mouseWorldPosition);
        RotateGun(angle);
    }

    private void RotateGun(float angle) {
        if (angle < 90 && angle > -90) {
            roatatePoint.eulerAngles = new Vector3(180, 0, -angle + 180);
        } else {
            roatatePoint.eulerAngles = new Vector3(0, 0, angle + 180);
        }
    }
}