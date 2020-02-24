using System;
using UnityEngine;

public class PlayerHealthUpdateArgs : EventArgs {
	public float CurrentHealth { get; set; }
	public float MaxHealth { get; set; }
}

public class PlayerHealthController : HealthController {

	private float MaxHealth { get; set; }

	public static event EventHandler<PlayerHealthUpdateArgs> OnPlayerHealthUpdate;

	/// <summary>
	/// Adds damage to the player health and trigger health event
	/// </summary>
	/// <param name="dmg"></param>
	override public void ApplyDamage(float dmg) {
		var args = new PlayerHealthUpdateArgs();
		this.EntityHealth -= dmg;
		args.CurrentHealth = EntityHealth;
		args.MaxHealth = MaxHealth;
		PlayerHealthController.OnPlayerHealthUpdate?.Invoke(this, args);
		this.CheckIfDead();
	}

	private void Awake() {
		this.MaxHealth = this.EntityHealth;
	}

}