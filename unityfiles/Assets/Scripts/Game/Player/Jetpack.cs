using UnityEngine;

/// <summary>
/// Simple jetpack script which adds force to positive Y axis, 
/// and stops adding force when hitting a set Y velocity.
/// </summary>
public class Jetpack : MonoBehaviour {
    // The force to add (x, y direction and magnitude)
    // Should be treated as newtons
    [SerializeField]
    private Vector2 force = new Vector2(0, 200);

    // The maximum velocity that can be achieved.
    [SerializeField]
    private int maxYVelocity = 5;

    // The rigidbody that is added force to.
    [SerializeField]
    private Rigidbody2D massToAffect;

    [SerializeField]
    private ParticleSystem flames;

    private Animator animator;
    private static readonly int Flying = Animator.StringToHash("Flying");

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        if (animator == null) {
            throw new MissingComponentException("Animator is missing");
        }

        this.flames.Stop();
    }

    private void FixedUpdate() {
        if (Input.GetAxis("Vertical") > 0) {
            AddForce();
            if (!this.flames.isEmitting)this.flames.Play();
        } else if (this.flames.isEmitting) {
            this.flames.Stop();
        }
    }

    /// <summary>
    /// Adds force to rigidbody as long as the selected key is pressed,
    /// until the maximum velocity is reached.
    /// </summary>
    private void AddForce() {
        if (this.massToAffect.velocity.y <= this.maxYVelocity) {
            this.massToAffect.AddForce(this.force);
        }
    }

    private void Update() {
        if (Input.GetButtonDown("Vertical")) {
            animator.SetBool(Flying, true);
        }

        if (Input.GetButtonUp("Vertical")) {
            animator.SetBool(Flying, false);
        }
    }
}