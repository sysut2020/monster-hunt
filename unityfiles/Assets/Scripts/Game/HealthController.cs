using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;
using UnityEngine;

/// <summary>
/// controlling an entity's health. can give or take away health
/// and keeping track of wether or not the entity is dead
/// </summary>
[RequireComponent (typeof (IDamageable))]
public class HealthController : MonoBehaviour {
    [Tooltip ("the amount of health a given entity has.")]
    [SerializeField]
    private float entityHealth = 1;

    [Tooltip ("Can the entity be healed after death")]
    [SerializeField]
    private bool canRevive = false;

    private bool isDead = false;

    private IDamageable damageable;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start () {
        this.damageable = this.gameObject.GetComponent<IDamageable> ();
    }

    // -- properties -- //

    public float EntityHealth {
        get { return this.entityHealth; }
        set { this.entityHealth = value; }
    }

    public bool IsDead {
        get { return this.isDead; }
        internal set { this.isDead = value; }
    }

    // -- public -- //

    /// <summary>
    /// Reduces the entity's Health by a given value
    /// </summary>
    /// <param name="dmg">The amount of damage given to the entity</param>
    public void ApplyDamage (float dmg) {
        // TODO: Possible debonce
        this.entityHealth = this.entityHealth - dmg;

        this.CheckIfDead ();
    }

    /// <summary>
    /// Increses the entity's health by the given value
    /// if the entity is dead check if it is Revive
    /// </summary>
    /// <param name="healing">The amount of health given to the entity</param>
    public void ApplyHealing (float healing) {
        // TODO: Possible debonce 

        if (!this.isDead || canRevive) {
            this.entityHealth = this.entityHealth + healing;
        }
    }

    // -- private -- // 

    /// <summary>
    /// Checks if the entity's health is below 0
    /// if it is set the isDead to true
    /// if not set it to false
    /// </summary>
    private void CheckIfDead () {
        if (this.entityHealth <= 0f && damageable != null) {
            this.damageable.Dead ();
            this.IsDead = true;

        } else {
            this.isDead = false;
        }
    }
}