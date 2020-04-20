using UnityEngine;
using UnityEngine.Serialization;

public abstract class HealthController : MonoBehaviour, IDamageable {
    [FormerlySerializedAs("entityStartHealth")]
    [Tooltip("the amount of health a given entity has.")]
    [SerializeField]
    protected float startHealth = 1;

    [SerializeField]
    private float health;
    protected float Health { get => health; set => health = value; }

    protected IKillable killable;

    protected IDamageNotifyable[] Notifyables { get; private set; } = {};

    public float StartHealth {
        get { return this.startHealth; }
        set { this.startHealth = value; }
    }

    public bool IsDead { get; internal set; } = false;

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
    /// if it is set the isDead to true and notify attached killable object,
    /// else set it to false
    /// </summary>
    protected void CheckIfDead() {
        if (this.Health <= 0f) {
            this.killable?.IsDead();
            this.IsDead = true;
        } else {
            this.IsDead = false;
        }
    }

    void Awake() {
        TryGetComponent<IKillable>(out killable);
        this.Notifyables = GetComponents<IDamageNotifyable>();
    }
}