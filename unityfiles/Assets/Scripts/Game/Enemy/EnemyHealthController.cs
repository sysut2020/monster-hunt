using UnityEngine;

/// <summary>
/// Controlling an enemy's health. can give or take away health
/// and keeping track of wetter or not the entity is dead
/// </summary>
[RequireComponent(typeof(IDamageable))]
public class EnemyHealthController : HealthController {

    [SerializeField]
    private EnemyHealthBarGUIController healthBarGuiController;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        if (healthBarGuiController == null) {
            throw new MissingComponentException("Missing EnemyHealthBarGUIController");
        }
        Health = startHealth;
        healthBarGuiController.SetStartHealth(startHealth);
    }

    /// <summary>
    /// Reduces the entity's Health by a given value
    /// </summary>
    /// <param name="dmg">The amount of damage given to the entity</param>
    public override void ApplyDamage(float dmg) {
        Health -= dmg;
        if (this.Health > this.StartHealth){
            this.Health = StartHealth;    
        }
        healthBarGuiController.UpdateHealthBar(Health);
        this.CheckIfDead();

        foreach (var notifyable in Notifyables) {
            notifyable.Damaged();
        }

    }

    /// <summary>
    /// Increases the entity's health by the given value
    /// if the entity is dead check if it is Revive
    /// </summary>
    /// <param name="healing">The amount of health given to the entity</param>
    public override void ApplyHealing(float healing) {
        if (!this.IsDead) {
            Health += healing;
            healthBarGuiController.UpdateHealthBar(Health);
        }
    }
}