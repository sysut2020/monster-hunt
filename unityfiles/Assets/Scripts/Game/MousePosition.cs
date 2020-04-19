using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition {
    // Returns a vector from the position that our aim are based of, to the mouse position
    public Vector3 MouseWorldPosition(GameObject aimDirectionPoint) {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition -
            aimDirectionPoint.transform.position);
    }
}