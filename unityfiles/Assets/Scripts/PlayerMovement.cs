using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    //Character controller script 
    private CharacterController2D characterController2D;
    public CharacterController2D CharacterController2D{
        get => characterController2D;
        set => characterController2D = value;
    }

    private float runSpeed = 40;
    public float RunSpeed{
        get => runSpeed;
        set => runSpeed = value;
    }

    //Horizontal movement speed of the player
    private float horizontalMove = 7000;
    public float HorizontalMove{
        get => horizontalMove;
        set => horizontalMove = value;
    }

    //If the player is jumping or not
    private bool jump;
    public bool Jump{
        get => jump;
        set => jump = value;
    }

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
        characterController2D.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
