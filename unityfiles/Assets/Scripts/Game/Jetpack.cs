using System.Collections;
using System.Collections.Generic;
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

    // The maximum velicity that can be achived.
    [SerializeField]
    private int maxYvelocity = 5;

    // The rigidbody that is added force to.
    [SerializeField]
    private Rigidbody2D massToAffect;

    // The key to press to activate the jetpack force
    [SerializeField]
    private KeyCode controllButton;

    private void Update() {
        if (Input.GetKey(controllButton)) {
            AddForce();
        }
    }

    /// <summary>
    /// Adds force to rigidbody as long as the selected key is pressed,
    /// until the maximum velocity is reached.
    /// </summary>
    private void AddForce() {

        if (this.massToAffect.velocity.y <= this.maxYvelocity) {
            this.massToAffect.AddForce(this.force);
        }

    }
}