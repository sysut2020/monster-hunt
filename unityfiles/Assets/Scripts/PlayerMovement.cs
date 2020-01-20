using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    //Character controller script 
    public CharacterController2D controller2D;
    public float runSpeed = 40;
    //Horizontal movement speed of the player
    public float horizontalMove = 7000;
    //If the player is jumping or not
    public bool jump = false;
    
    void Update() {
        //Checks if the player is pressing the right of left arrow key
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        
        //Checks if the player is pressing up arrow key for jumping
        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
    }

    void FixedUpdate() {
        //Moves the player accordingly to the pressed arrow keys
        controller2D.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
