using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerHealthUpdateArgs : EventArgs {
	public float CurrentHealth { get; set; }
	public float MaxHealth { get; set; }
}
public class PlayerLivesUpdateArgs : EventArgs {
	public int CurrentLives { get; set; }
}

public class PlayerHealthController : HealthController {

	public static event EventHandler<PlayerHealthUpdateArgs> OnPlayerHealthUpdate;
	public static event EventHandler<PlayerLivesUpdateArgs> OnPlayerLivesUpdate;

	private float MaxHealth { get; set; }

	[SerializeField]
	private int startLives = 3;
	private int StartLives {
		get { return startLives; }
		set { startLives = value; }
	}

	private int CurrentLives {
		get;
		set;
	}

	// Timer for when last was hit.
	private float lastHitTimer;

	// Guard time before can take damage again
	private float guardTime = 0.2f;

	/// <summary>
	/// Adds damage to the player health and trigger health update event.
	/// If the player died, update lives.
	/// </summary>
	/// <param name="dmg"></param>
	override public void ApplyDamage(float dmg) {
		if (!this.CanTakeDamage()) { return; }
		this.Health -= dmg;
		this.InvokeHealthUpdate();
		this.CheckIfDead();
		if (this.IsDead) {
			ReduceLivesAndResetHealth();
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

	public override void ApplyHealing(float healing) {
		this.Health += Mathf.Clamp(healing, 0, MaxHealth);
		this.InvokeHealthUpdate();
	}

	private void Start() {
		this.MaxHealth = this.Health = this.StartHealth;
		this.CurrentLives = this.StartLives;
		this.InvokeLivesUpdate();

	}
}