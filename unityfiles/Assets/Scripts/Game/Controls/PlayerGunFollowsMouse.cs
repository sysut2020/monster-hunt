using System;
using UnityEngine;

public class PlayerGunFollowsMouse : MonoBehaviour {
    
    [SerializeField]
    private Transform rotatePoint;

    private MousePosition mousePosition;


    bool isFacingRight = true;
    

    // -- private -- //

    private void RotateGun(float angle) {
        if (angle < 90 && angle > -90) {
            rotatePoint.eulerAngles = new Vector3(180, 0, -angle + 180);
        } else {
            rotatePoint.eulerAngles = new Vector3(0, 0, angle + 180);
        }
    }

    private void MaybeFlipCharacter (float playerGunAngle) {

        if (playerGunAngle > -90 || playerGunAngle < 90) {
            if (this.isFacingRight){
                transform.rotation = Quaternion.Euler (new Vector3 (0, 180f, 0));

                this.isFacingRight = false;
            }
			
		}
		if (playerGunAngle < -90 || playerGunAngle > 90) {
            if (!this.isFacingRight){
                transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
                
                this.isFacingRight = true;
            }
			
		}
	}


    // -- unity -- //
    private void Start() {
        mousePosition = new MousePosition();
    }

    void Update() {
        // Creates a vector from the position that our aim are based of, to the mouse position
        Vector3 mouseWorldPosition = mousePosition.MouseWorldPosition(rotatePoint.gameObject);
        float angle = VectorHelper.AngleBetweenVector2D(rotatePoint.position, mouseWorldPosition);
        RotateGun(angle);
        MaybeFlipCharacter(angle);



        



    }
}