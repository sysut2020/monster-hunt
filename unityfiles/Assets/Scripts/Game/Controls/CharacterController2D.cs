using System;
using UnityEngine;
/**
 * All credits given to Brackeys GitHub: https://github.com/Brackeys/2D-Movement
 * This project is only intended to use for educational purposes, not to be
 * sold or re-distriubted to make money.
 */
public class CharacterController2D : MonoBehaviour {

	[SerializeField]
	private float jumpForce = 400f;

	[Range(0, .3f)]
	[SerializeField]
	private float movementSmoothing = .05f;

	// Whether or not a player can steer while jumping
	[SerializeField]
	private bool airControl;

	// A mask determining what is ground to the character
	[SerializeField]
	private LayerMask whatIsGround;


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
	

	/// <summary>
	/// Moves the player with a force in direction given by movement parameter
	/// </summary>
	/// <param name="movement">x axis direction</param>
	public void Move(float movement) {
		if (airControl) {
			Vector3 targetVelocity = new Vector2(movement, playerRigidbody2D.velocity.y);
			playerRigidbody2D.velocity = Vector3.SmoothDamp(playerRigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
		}
	}

}