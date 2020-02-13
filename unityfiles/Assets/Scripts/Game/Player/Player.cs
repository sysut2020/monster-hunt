using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// this class describes the player
/// </summary>
public class Player : MonoBehaviour {

    [SerializeField]
    private int playerLives;

    private HealthController playerHealthController;

    private PlayerInventory playerInventory;

    private Animator animator;

    private PlayerWeaponController playerWeaponController;

    // -- properties -- //
    public int PlayerLives {
        get => playerLives;
        set => playerLives = value;
    }

    public HealthController PlayerHealthController {
        get => playerHealthController;
        internal set => this.playerHealthController = value;
    }

    public PlayerInventory PlayerInventory {
        get => playerInventory;
        internal set => this.playerInventory = value;
    }

    public PlayerWeaponController PlayerWeaponController {
        get => playerWeaponController;
        internal set => this.playerWeaponController = value;
    }

    // -- public -- //

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    /// <summary>
    /// Gives a pickup to the player
    /// </summary>
    /// <param name="pickup">the pickup to give to the player</param>
    public void GivePickup(IPowerUp pickup) {
        // activate
        pickup.ApplyEffect(this);
        // if effect is persistent give it to the watcher
        if (pickup is IEffectPowerUp) {
            IEffectPowerUp equalTypeEffect = playerInventory.ActivePickups.Find(effect => effect.GetPickupName().Equals(pickup.GetPickupName()));
            if (equalTypeEffect != null) {
                // if it exists extend the existing
                equalTypeEffect.ExtendEffect(equalTypeEffect);
            } else {
                // else just bop it on the end
                this.PlayerInventory.AddEffectPickup(pickup as IEffectPowerUp);
            }
        }
    }

    /// <summary>
    /// Gives money to the player
    /// </summary>
    /// <param name="money">the amount of money to give the player</param>
    public void GiveMoney(int money) {
        this.PlayerInventory.AddMoney(money);
    }

    /// <summary>
    /// Gives a letter to the player
    /// </summary>
    /// <param name="letter">the letter to give to the player</param>        
    public void GiveLetter(String letter) {
        this.playerInventory.AddLetter(letter);
    }


    // -- private -- // 

    
    /// <summary>
    /// iterates through the active effects and checks if any one of them are done
    /// if they are the effect is cleaned out and removed
    /// </summary>
    private void UpdateEffects() {
        foreach (IEffectPowerUp effect in this.playerInventory.ActivePickups.Reverse<IEffectPowerUp>()) {
            if (effect.IsEffectFinished()) {
                effect.Cleanup();
                this.playerInventory.RemoveEffectPickup(effect);
            }
        }
    }

    

    // -- unity -- //

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        UpdateEffects();
    }

    /// <summary>
    /// Checks for collisions with other objects
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other) {
        var enemy = other.gameObject.GetComponent<Enemy>();

        try {
            if (enemy.IsAttacking) {
                animator.SetTrigger("Damage");
            }

        } catch (System.Exception) { }

    }
}