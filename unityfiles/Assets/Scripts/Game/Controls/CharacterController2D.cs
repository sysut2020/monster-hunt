using System;
using UnityEngine;
/**
 * All credits given to Brackeys GitHub: https://github.com/Brackeys/2D-Movement
 * This project is only intended to use for educational purposes, not to be
 * sold or re-distriubted to make money.
 */
public class CharacterController2D : MonoBehaviour {

	// Amount of force added when the player jumps.
	[SerializeField]
	private float jumpForce = 400f;

	// How much to smooth out the movement
	[Range(0, .3f)]
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

	// The point that we want our aim to be based of
	[SerializeField]
	private GameObject characterCenter;

	// Radius of the overlap circle to determine if grounded
	const float GROUNDED_RADIUS = .2f;

	// Whether or not the player is grounded.
	private bool grounded;

	private Rigidbody2D playerRigidbody2D;

	private Vector3 velocity = Vector3.zero;
	private MousePosition mousePosition;

	private void Awake() {
		playerRigidbody2D = GetComponent<Rigidbody2D>();
		mousePosition = new MousePosition();
	}

	private void FixedUpdate() {
		Vector3 mouseWorldPosition = mousePosition.MouseWorldPosition(characterCenter);
		grounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, GROUNDED_RADIUS, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				grounded = true;
			}
		}
	}

	/// <summary>
	/// Moves the player with a force in direction given by movement parameter
	/// </summary>
	/// <param name="movement">x axis direction</param>
	public void Move(float movement) {
		if (grounded || airControl) {
			Vector3 targetVelocity = new Vector2(movement, playerRigidbody2D.velocity.y);
			playerRigidbody2D.velocity = Vector3.SmoothDamp(playerRigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
		}
	}

	/// <summary>
	/// Adds uppward force to the the rigidbody.
	/// </summary>
	public void Jump() {
		if (grounded) {
			playerRigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}
	}

}