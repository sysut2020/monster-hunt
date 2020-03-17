using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class HealthController : MonoBehaviour {
    [FormerlySerializedAs("entityStartHealth")]
    [Tooltip("the amount of health a given entity has.")]
    [SerializeField]
    protected float startHealth = 1;
    private bool isDead = false;
    protected float health;


    protected IDamageable damageable;

    public float StartHealth {
        get { return this.startHealth; }
        set { this.startHealth = value; }
    }

    private void Awake() {
        this.damageable = this.gameObject.GetComponent<IDamageable>();
    }

    public bool IsDead {
        get { return this.isDead; }
        internal set { this.isDead = value; }
    }

    /// <summary>
    /// Reduces the entity's Health by a given value
    /// </summary>
    /// <param name="dmg">The amount of damage given to the entity</param>
    public abstract void ApplyDamage(float dmg);

    /// <summary>
    /// Increases the entity's health by the given value
    /// if the entity is dead check if it is Revive
    /// </summary>
    /// <param name="healing">The amount of health given to the entity</param>
    public abstract void ApplyHealing(float healing);
    
    /// <summary>
    /// Checks if the entity's health is below 0
    /// if it is set the isDead to true
    /// if not set it to false
    /// </summary>
    protected void CheckIfDead() {
        if (this.health <= 0f && damageable != null) {
            this.damageable.Dead();
            this.IsDead = true;
        } else {
            this.IsDead = false;
        }
    }
}