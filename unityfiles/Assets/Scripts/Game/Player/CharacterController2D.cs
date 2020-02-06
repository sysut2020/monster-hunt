using System;
using UnityEngine;
/**
 * All credits given to Brackeys GitHub: https://github.com/Brackeys/2D-Movement
 * This project is only intended to use for educational purposes, not to be
 * sold or re-distriubted to make money.
 */
public class CharacterController2D : MonoBehaviour
{

	// Amount of force added when the player jumps.
	[SerializeField]
	private float jumpForce = 400f;
	
	// How much to smooth out the movement
	[Range (0, .3f)]
	[SerializeField]
	private float movementSmoothing = .05f;

	// Whether or not a player can steer while jumping
	[SerializeField]
	private bool airControl;

	// A mask determining what is ground to the character
	[SerializeField]
	private LayerMask whatIsGround;

	// A position marking where to check if the player is grounded.
	[SerializeField]
	private Transform groundCheck;

	// A position marking where to check for ceilings
	[SerializeField]
	private Transform ceilingCheck;

	// A collider that will be disabled when crouching
	[SerializeField] private Collider2D crouchDisableCollider;
	// The point that we want our aim to be based of
	[SerializeField] private GameObject characterCenter;

	// Radius of the overlap circle to determine if grounded
	const float GROUNDED_RADIUS = .2f;

	// Whether or not the player is grounded.
	private bool grounded;

	// Radius of the overlap circle to determine if the player can stand up
	const float CEILING_RADIUS = .2f;
	private Rigidbody2D playerRigidbody2D;

	// For determining which way the player is currently facing.
	private bool facingRight = true;
	private Vector3 velocity = Vector3.zero;
	private MousePosition mousePosition;
	private GunAngle gunAngle;
	private float angle;
	
	private void Awake () {
		playerRigidbody2D = GetComponent<Rigidbody2D>();
		mousePosition = new MousePosition();
		gunAngle = new GunAngle();
	}

	private void FixedUpdate () {
		Vector3 mouseWorldPosition = mousePosition.MouseWorldPosition (characterCenter);
		angle = gunAngle.GetGunAngle(transform.position, mouseWorldPosition);

		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll (groundCheck.position, GROUNDED_RADIUS, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				grounded = true;
			}
		}
	}

	public void Move (float movement, bool crouch, bool jump) {
		//only control the player if grounded or airControl is turned on
		if (grounded || airControl) {
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2 (movement * 10f, playerRigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			playerRigidbody2D.velocity = Vector3.SmoothDamp (playerRigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
			
			CheckFlipBoundary();
			Flip();
		}
		// If the player should jump...
		if (grounded && jump) {
			// Add a vertical force to the player.
			grounded = false;
			playerRigidbody2D.AddForce (new Vector2 (0f, jumpForce));
		}
	}

	private void Flip () {
		// If the gun is facing to the right, flip character left
		if (!facingRight) {
			transform.rotation = Quaternion.Euler (new Vector3 (0, 180f, 0));
		}
		// If the gun is facing to the right, flip character right
		if (facingRight) {
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		}
	}

	private void CheckFlipBoundary() {
		// If the gun is facing to the left, make facingRight false 
		if (angle > -90 || angle < 90) {
			facingRight = false;
		}
		// If the gun is facing to the right, make facingRight true
		if (angle < -90 || angle > 90) {
			facingRight = true;
		}
	}
}