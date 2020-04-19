using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for bubbling up the attack trigger event since the attack trigger
/// collider is in deep nested objects.
/// </summary>
public class EnemyAttackHitNotifyer : MonoBehaviour {
    private EnemyBehaviour enemyBehaviour;

    private void Awake() {
        this.enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
    }

    /// <summary>
    /// Checks if the targeted is am IDamagble type, and notify the handler script
    /// if it is.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent(out IDamageable damageable)) {
            enemyBehaviour.HitTarget();
        }
    }
}