using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[SerializeField]
	private float runSpeed = 40;

	[SerializeField]
	private float horizontalMove = 0;

	[Range(0, .3f)]
	[SerializeField]
	private float movementSmoothing = .05f;

	private Rigidbody2D playerRigidbody2D;
	private Vector3 velocity = Vector3.zero;

	private Animator animator;
	private static readonly int Walking = Animator.StringToHash("Walking");

	private void Awake() {
		playerRigidbody2D = GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
		if (animator == null) {
			throw new MissingComponentException("Player animator not found");
		}
	}

	/// <summary>
	/// Moves the player with a force in direction given by movement parameter
	/// </summary>
	/// <param name="movement">x axis direction</param>
	private void Move(float movement) {
		Vector3 targetVelocity = new Vector2(movement, playerRigidbody2D.velocity.y);
		playerRigidbody2D.velocity =
			Vector3.SmoothDamp(playerRigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
	}

	void Update() {
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Horizontal")) {
			animator.SetBool(Walking, true);
		}

		if (Input.GetButtonUp("Horizontal")) {
			animator.SetBool(Walking, false);
		}
	}

	void FixedUpdate() {
		this.Move(horizontalMove * Time.fixedDeltaTime);
	}
}