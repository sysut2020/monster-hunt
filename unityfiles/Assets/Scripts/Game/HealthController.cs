using UnityEngine;

/// <summary>
/// controlling an entity's health. can give or take away health
/// and keeping track of wetter or not the entity is dead
/// </summary>
[RequireComponent(typeof(IDamageable))]
public class HealthController : MonoBehaviour {
    [Tooltip("the amount of health a given entity has.")]
    [SerializeField]
    private float entityStartHealth = 1;

    private float health;
    private bool isDead = false;
    private IDamageable damageable;
    private EnemyHealthBarGUIController healthBarGuiController;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        this.damageable = this.gameObject.GetComponent<IDamageable>();
        health = entityStartHealth;

        healthBarGuiController = (EnemyHealthBarGUIController) FindObjectOfType(typeof(EnemyHealthBarGUIController));
        
        if (healthBarGuiController == null) {
            throw new MissingComponentException("Missing EnemyHealthBarGUIController"); // todo this will always be thrown from the player
        }

        healthBarGuiController.StartHealth = entityStartHealth;
    }

    // -- properties -- //

    public float EntityStartHealth {
        get { return this.entityStartHealth; }
        set { this.entityStartHealth = value; }
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
    public virtual void ApplyDamage(float dmg) {
        health -= dmg;
        healthBarGuiController.UpdateHealthBar(health);
        this.CheckIfDead();
    }

    /// <summary>
    /// Increases the entity's health by the given value
    /// if the entity is dead check if it is Revive
    /// </summary>
    /// <param name="healing">The amount of health given to the entity</param>
    public void ApplyHealing(float healing) {
        if (!this.isDead) {
            health += healing;
            healthBarGuiController.UpdateHealthBar(health);
        }
    }

    // -- private -- // 

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