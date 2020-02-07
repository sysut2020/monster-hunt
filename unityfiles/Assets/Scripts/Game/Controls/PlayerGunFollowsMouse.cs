using System;
using UnityEngine;

public class PlayerGunFollowsMouse : MonoBehaviour {

    [SerializeField]
    private Transform rotatePoint;

    public Transform guuuuun;

    /// <summary>
    /// The point that is used to check if mouse is on left/right side of
    /// </summary>
    [SerializeField]
    private Transform crossingPoint;

    /// <summary>
    /// The point that is used to check if mouse is on left/right side of
    /// </summary>
    [SerializeField]
    private Transform qwe;

    private MousePosition mousePosition;

    float angle2;

    bool mouseOnRightSide = true;

    // -- private -- //

    private void RotateGun() {

        // float angle = VectorHelper.AngleBetweenVector2D(rotatePoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float angle2 = Vector3.Angle(rotatePoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float angle = Quaternion.Angle(rotatePoint.rotation, guuuuun.rotation);

        Debug.Log(angle);
        // rotatePoint.rotation = Quaternion.Angle();
        if (angle < 90 && angle > -90) {
            rotatePoint.eulerAngles = new Vector3(180, 0, angle);
        } else {
            rotatePoint.eulerAngles = new Vector3(0, 0, angle);
        }
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
        angle2 = Vector2.Angle(this.rotatePoint.position, this.guuuuun.transform.position);
        Debug.Log(angle2);
        mousePosition = new MousePosition();
    }

    void Update() {
        Debug.DrawLine(this.rotatePoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.magenta);
        // Creates a vector from the position that our aim are based of, to the mouse position
        Vector3 mouseWorldPosition = mousePosition.MouseWorldPosition(rotatePoint.gameObject);
        // float angle = VectorHelper.AngleBetweenVector2D(rotatePoint.position, mouseWorldPosition);
        RotateGun();
        if (this.IsMouseOnOtherSideOfCrossing()) {
            FlipCharacter();
        }
    }
}