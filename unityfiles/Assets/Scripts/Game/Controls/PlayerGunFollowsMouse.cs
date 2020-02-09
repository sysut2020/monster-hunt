using System;
using UnityEngine;

public class PlayerGunFollowsMouse : MonoBehaviour {

    [SerializeField]
    private Transform rotatePoint;
    [SerializeField]
    private Transform FirePoint;

    /// <summary>
    /// The point that is used to check if mouse is on left/right side of
    /// </summary>
    [SerializeField]
    private Transform crossingPoint;

    private MousePosition mousePosition;
    private AimControl aimControl;

    float angle2;

    bool mouseOnRightSide = true;

    // -- private -- //

    private void RotateGun() {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotatePoint.transform.rotation = Quaternion.Euler(aimControl.GetAngle(mousePosition));
    }

    /// <summary>
    /// Check if the mouse have crossed over the crossing point,
    /// returns true if it has, else false.
    /// </summary>
    /// <returns>true if crossed, else false</returns>
    private bool IsMouseOnOtherSideOfCrossing() {
        float mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float crossingPointX = this.crossingPoint.position.x;
        bool crossed = this.mouseOnRightSide;
        if ((mousePositionX > crossingPointX) && !mouseOnRightSide) {
            mouseOnRightSide = true;
        } else if ((mousePositionX < crossingPointX) && mouseOnRightSide) {
            mouseOnRightSide = false;
        }
        return crossed != this.mouseOnRightSide;
    }

    private void FlipCharacter() {
        this.crossingPoint.Rotate(0, 180, 0);
    }

    // -- unity -- //
    private void Start() {
        mousePosition = new MousePosition();
        this.aimControl = new AimControl(FirePoint.gameObject, rotatePoint.transform);
    }

    void FixedUpdate() {
        RotateGun();
        if (this.IsMouseOnOtherSideOfCrossing()) {
            FlipCharacter();
        }
    }
}