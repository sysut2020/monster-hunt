using UnityEngine;
/**
 * All credits given to Brackeys GitHub: https://github.com/Brackeys/2D-Movement
 * This project is only intended to use for educational purposes, not to be
 * sold or re-distriubted to make money.
 */
public class CharacterController2D : MonoBehaviour {
	// Amount of force added when the player jumps.
	[SerializeField] private float jumpForce = 400f;
	// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;	
	// How much to smooth out the movement
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
	// Whether or not a player can steer while jumping;
	[SerializeField] private bool airControl;		
	// A mask determining what is ground to the character
	[SerializeField] private LayerMask whatIsGround;
	// A position marking where to check if the player is grounded.
	[SerializeField] public Transform groundCheck;	
	// A position marking where to check for ceilings
	[SerializeField] public Transform ceilingCheck;
	// A collider that will be disabled when crouching
	[SerializeField] private Collider2D crouchDisableCollider;				
	
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

	private void Awake() {
		playerRigidbody2D = GetComponent<Rigidbody2D>();
	}


	private void FixedUpdate() {
		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, GROUNDED_RADIUS, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				grounded = true;
			}
		}
	}


	public void Move(float movement, bool crouch, bool jump) {
		// If crouching, check to see if the character can stand up
		if (!crouch) {
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(ceilingCheck.position, CEILING_RADIUS, whatIsGround)) {
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (grounded || airControl) {

			// If crouching
			if (crouch) {
				// Reduce the speed by the crouchSpeed multiplier
				movement *= crouchSpeed;

				// Disable one of the colliders when crouching
				if (crouchDisableCollider != null) {
					crouchDisableCollider.enabled = false;
				}
			} else {
				// Enable the collider when not crouching
				if (crouchDisableCollider != null) {
					crouchDisableCollider.enabled = true;
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(movement * 10f, playerRigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			playerRigidbody2D.velocity = Vector3.SmoothDamp(playerRigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (movement > 0 && !facingRight) {
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (movement < 0 && facingRight) {
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (grounded && jump) {
			// Add a vertical force to the player.
			grounded = false;
			playerRigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}
	}


	private void Flip() {
		// If character is moving to the left, rotate left
		if (Input.GetAxisRaw("Horizontal") < 0) {
			facingRight = false;
			transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
		}
		
		// If character is moving to the right, rotate right
		if (Input.GetAxisRaw("Horizontal") > 0) {
			facingRight = true;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		}
	}
}
