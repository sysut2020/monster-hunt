using System;
using UnityEngine;

public class PlayerHealthUpdateArgs : EventArgs {
	public float CurrentHealth { get; set; }
	public float MaxHealth { get; set; }
}

public class PlayerHealthController : HealthControllerBase {

	private float MaxHealth { get; set; }

	public static event EventHandler<PlayerHealthUpdateArgs> OnPlayerHealthUpdate;

	/// <summary>
	/// Adds damage to the player health and trigger health event
	/// </summary>
	/// <param name="dmg"></param>
	override public void ApplyDamage(float dmg) {
		var args = new PlayerHealthUpdateArgs();
		this.EntityStartHealth -= dmg;
		args.CurrentHealth = EntityStartHealth;
		args.MaxHealth = MaxHealth;
		PlayerHealthController.OnPlayerHealthUpdate?.Invoke(this, args);
		this.CheckIfDead();
	}

	public override void ApplyHealing(float healing) {
		throw new NotImplementedException();
	}

	private void Awake() {
		this.MaxHealth = this.EntityStartHealth;
	}

}