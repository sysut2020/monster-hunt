using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerMovement : MonoBehaviour {

    public CharacterController2D controller2D;

    public float runSpeed = 40;
    
    public float horizontalMove = 7000;
    public bool jump = false;
    void Update(){
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        
        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
    }

    private void FixedUpdate(){
        controller2D.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
