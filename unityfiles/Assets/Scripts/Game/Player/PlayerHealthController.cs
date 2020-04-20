using System;
using UnityEngine;

public class PlayerHealthUpdateArgs : EventArgs {
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
}
public class PlayerLivesUpdateArgs : EventArgs {
    public int CurrentLives { get; set; }
}

public class PlayerHealthController : HealthController, IObstacleDamagable {

    public static event EventHandler<PlayerHealthUpdateArgs> OnPlayerHealthUpdate;
    public static event EventHandler<PlayerLivesUpdateArgs> OnPlayerLivesUpdate;

    private float MaxHealth { get; set; }

    [SerializeField]
    private int startLives = 3;
    private int StartLives {
        get { return startLives; }
        set { startLives = value; }
    }

    [SerializeField]
    private int currentLives;
    public int CurrentLives { get => currentLives; set => currentLives = value; }

    // Timer for when last was hit.
    private float lastHitTimer;

    // Guard time before can take damage again
    private float guardTime = 0.2f;

    /// <summary>
    /// Remove the provided damage amount from the players health pool
    /// </summary>
    /// <param name="damage"></param>
    override public void ApplyDamage(float damage) {
        this.ReduceHealth(damage);

    }

    /// <summary>
    /// Adds provided healing amount to the players health
    /// </summary>
    /// <param name="healing">amount of healing to give</param>
    public override void ApplyHealing(float healing) {
        this.Health += Mathf.Clamp(healing, 0, MaxHealth);
        this.InvokeHealthUpdate();
    }

    /// <summary>
    /// Remove the provided damage amount from the players health pool
    /// </summary>
    /// <param name="damage">damage amount</param>
    public void ApplyObstacleDamage(float damage) {
        this.ReduceHealth(damage);
    }

    /// <summary>
    /// Reduces the health by given amount, if the player can take damage.
    /// Trigger health update event to notify health change.
    /// </summary>
    /// <param name="reduceAmount">amount to remove</param>
    private void ReduceHealth(float amount) {
        if (!this.CanTakeDamage()) { return; }
        this.Health -= amount;
        this.InvokeHealthUpdate();
        this.CheckIfDead();
        if (this.IsDead) {
            ReduceLivesAndResetHealth();
        } else {
            foreach (var notifyable in Notifyables) {
                notifyable?.Damaged();
            }
        }
    }

    /// <summary>
    /// Reduce lives by one and invoke lives update event, and reset the players
    /// health if there are more lives left.
    /// </summary>
    private void ReduceLivesAndResetHealth() {
        this.CurrentLives--;
        this.InvokeLivesUpdate();
        if (CurrentLives > 0) {
            this.IsDead = false;
            this.ApplyHealing(MaxHealth);
        }
    }

    // Guard timer for multi damage, like if player hits damage collider X frames in a row.
    private bool CanTakeDamage() {
        if (Time.time < this.lastHitTimer) { return false; }
        this.lastHitTimer = Time.time + this.guardTime; // Adds 0.15 seconds guard time.
        return true;
    }

    private void InvokeHealthUpdate() {
        OnPlayerHealthUpdate?.Invoke(this, new PlayerHealthUpdateArgs { MaxHealth = this.MaxHealth, CurrentHealth = this.Health });
    }

    private void InvokeLivesUpdate() {
        OnPlayerLivesUpdate?.Invoke(this, new PlayerLivesUpdateArgs { CurrentLives = this.CurrentLives });
    }

    private void SubscribeToEvents() {
        LivesManager.OnPickupLivesUpdate += CallbacOnPickupLivesUpdate;
    }

    private void UnsubscribeFromEvents() {
        LivesManager.OnPickupLivesUpdate -= CallbacOnPickupLivesUpdate;
    }

    private void CallbacOnPickupLivesUpdate(object _, OnPickupLivesUpdateArgs args) {
        int newlives = this.CurrentLives + args.LivesToAdd;
        if (newlives > this.startLives) {
            newlives = this.startLives;
        }

        this.CurrentLives = newlives;
        this.InvokeLivesUpdate();
    }

    private void Start() {
        this.MaxHealth = this.Health = this.StartHealth;
        this.CurrentLives = this.StartLives;
        this.InvokeLivesUpdate();
        this.SubscribeToEvents();
    }

    private void OnDestroy() {
        this.UnsubscribeFromEvents();
    }
}