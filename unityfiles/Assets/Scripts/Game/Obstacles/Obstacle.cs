using UnityEngine;

/// <summary>
/// OBstacles are entities in the world which hinders the player, like spikes
/// that damages, posionous flowers etc.
/// Can register damage with either Triggers or collision (2D)
/// </summary>
public class Obstacle : MonoBehaviour {

    /// <summary>
    /// How much damage this obstacle will give
    /// </summary>
    [SerializeField]
    private float damage;

    public float Damage { get { return this.damage; } }

    /// <summary>
    /// Adds obstacle damage to the other component if it is of correct type.
    /// </summary>
    /// <param name="other">object colided with</param>
    private void OnTriggerEnter2D(Collider2D other) {
        this.TryDamage(other.gameObject);
    }

    /// <summary>
    /// Adds obstacle damage to the other component if it is of correct type.
    /// </summary>
    /// <param name="other">object colided with</param>
    private void OnCollisionEnter2D(Collision2D other) {
        this.TryDamage(other.gameObject);
    }

    /// <summary>
    /// Tries to find target type, and apply damage to it.
    /// </summary>
    /// <param name="target"></param>
    private void TryDamage(GameObject target) {
        if (target.TryGetComponent(out IObstacleDamagable damageble)) {
            damageble.ApplyObstacleDamage(this.Damage);
        }
    }

}