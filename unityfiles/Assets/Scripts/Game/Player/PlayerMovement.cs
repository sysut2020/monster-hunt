using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour {

    private CharacterController2D characterController2D;

    [SerializeField]
    private float runSpeed = 40;

    //Horizontal movement speed of the player
    [SerializeField]
    private float horizontalMove = 0;

    //If the player is jumping or not
    private bool jump;

    private void Start() {
        this.characterController2D = this.GetComponent<CharacterController2D>();
    }

    void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButtonDown("Jump")) {
            characterController2D.Jump();
        }
    }

    void FixedUpdate() {
        characterController2D.Move(horizontalMove * Time.fixedDeltaTime);
    }
}