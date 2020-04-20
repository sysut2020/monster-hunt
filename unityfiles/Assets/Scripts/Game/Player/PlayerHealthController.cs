using System;
using UnityEngine;

public class PlayerHealthUpdateArgs : EventArgs {
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
}
public class PlayerLivesUpdateArgs : EventArgs {
    public int CurrentLives { get; set; }
}

/// <summary>
/// Controls health of the player. Handles damage and healing events 
/// </summary>
public class PlayerHealthController : HealthController, IObstacleDamagable {

    public static event EventHandler<PlayerHealthUpdateArgs> OnPlayerHealthUpdate;
    public static event EventHandler<PlayerLivesUpdateArgs> OnPlayerLivesUpdate;

    private float MaxHealth { get; set; }

    /// <summary>
    /// The amount of starting life. This is also the maximum amount of life a player can have
    /// </summary>
    [SerializeField]
    private int startLives = 3;
    private int StartLives {
        get => startLives; 
    }

    [SerializeField]
    private int currentLives;
    public int CurrentLives { get => currentLives; set => currentLives = value; }

    /// <summary>
    /// Timer for when last was hit.
    /// </summary>
    private float lastHitTimer;

    /// <summary>
    /// Guard time before can take damage again
    /// </summary>
    private float guardTime = 0.2f;

    /// <summary>
    /// Remove the provided damage amount from the players health pool
    /// </summary>
    /// <param name="damage"></param>
    public override void ApplyDamage(float damage) {
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
    /// <param name="amount">amount to remove</param>
    private void ReduceHealth(float amount) {
        if (!this.CanTakeDamage()) { return; }
        this.Health -= amount;
        this.InvokeHealthUpdate();
        this.CheckIfDead();
        if (this.IsDead) {
            ReduceLivesAndResetHealth();
        } else {
            foreach (var notifiable in Notifyables) {
                notifiable?.Damaged();
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

    /// <summary>
    /// Checks if a player has taken damage recently.
    /// Guard timer for multi damage, like if player hits damage collider X frames in a row.
    /// </summary>
    /// <returns>Returns true if time since last damage taken is > than threshold. False if not </returns>
    private bool CanTakeDamage() {
        if (Time.time < this.lastHitTimer) { return false; }
        this.lastHitTimer = Time.time + this.guardTime;
        return true;
    }

    private void InvokeHealthUpdate() {
        OnPlayerHealthUpdate?.Invoke(this, new PlayerHealthUpdateArgs { MaxHealth = this.MaxHealth, CurrentHealth = this.Health });
    }

    private void InvokeLivesUpdate() {
        OnPlayerLivesUpdate?.Invoke(this, new PlayerLivesUpdateArgs { CurrentLives = this.CurrentLives });
    }

    private void SubscribeToEvents() {
        LivesManager.OnPickupLivesUpdate += CallbackOnPickupLivesUpdate;
    }

    private void UnsubscribeFromEvents() {
        LivesManager.OnPickupLivesUpdate -= CallbackOnPickupLivesUpdate;
    }

    private void CallbackOnPickupLivesUpdate(object _, OnPickupLivesUpdateArgs args) {
        int newLives = this.CurrentLives + args.LivesToAdd;
        if (newLives > this.startLives) {
            newLives = this.startLives;
        }

        this.CurrentLives = newLives;
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