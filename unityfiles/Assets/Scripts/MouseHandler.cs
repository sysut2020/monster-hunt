using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour {

    public Vector3 MouseWorldPosition(GameObject aimDirectionPoint) {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition + (Vector3.forward * 10f) -
                                              aimDirectionPoint.transform.position);
    }
}
