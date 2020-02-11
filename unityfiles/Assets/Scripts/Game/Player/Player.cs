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
    public void GivePickup (IPickup pickup) {
        // activate
        pickup.ApplyEffect (this);
        // if effect is persistent give it to the watcher
        if (pickup is IEffectPickup) {
            IEffectPickup equalTypeEffect = playerInventory.ActivePickups.Find (effect => effect.GetPickupName ().Equals (pickup.GetPickupName ()));
            if (equalTypeEffect != null) {
                // if it exists extend the existing
                equalTypeEffect.ExtendEffect (equalTypeEffect);
            } else {
                // else just bop it on the end
                this.PlayerInventory.AddEffectPickup (pickup as IEffectPickup);
            }
        }
    }

    /// <summary>
    /// Gives money to the player
    /// </summary>
    /// <param name="money">the amount of money to give the player</param>
    public void GiveMoney (int money) {
        this.PlayerInventory.AddMoney (money);
    }

    /// <summary>
    /// Gives a letter to the player
    /// </summary>
    /// <param name="letter">the letter to give to the player</param>        
    public void GiveLetter (Letter letter) {
        this.playerInventory.AddLetter (letter);
    }

    
    /// <summary>
    /// changes to the next weapon in the weapon list 
    /// if the end of the list is reached the index loops around
    /// </summary>
    /// <returns>The weapon controller of the new active weapon</returns>
    public void ChangeToNextWeapon(){
        GunController newGunController = this.PlayerWeaponController.ChangeToNextWeapon();
        this.AlertPowerupsOnWeaponChange(newGunController);
    }


    /// <summary>
    /// changes to the prev weapon in the weapon list 
    /// if the end of the list is reached the index loops around
    /// </summary>
    /// <returns>The weapon controller of the new active weapon</returns>
    public void ChangeToPrevWeapon(){
        GunController newGunController = this.PlayerWeaponController.ChangeToPrevWeapon();
        this.AlertPowerupsOnWeaponChange(newGunController);
    }

    /// <summary>
    /// Starts to fire the weapon
    /// </summary>
    public void StartFiring() {this.PlayerWeaponController.StartFiring();}

    /// <summary>
    /// Stop fireing the weapon
    /// </summary>
    public void StopFiring() {this.PlayerWeaponController.StopFiring();}

    /// <summary>
    /// Fires a bullet if able (not on cooldown since last shot)
    /// </summary>
    public void FireOnce() {this.PlayerWeaponController.FireOnce();}

    // -- private -- // 


    /// <summary>
    /// Alerts the power ups that the weapon has changed
    /// </summary>
    /// <param name="newGunC">The new gun controller to pass to the weapon</param>
    private void AlertPowerupsOnWeaponChange(GunController newGunC){
        foreach (IEffectPickup effect in this.PlayerInventory.ActivePickups.Reverse<IEffectPickup> ()) {
            if (effect is IWeaponEffectPickup ) {
                IWeaponEffectPickup weaponEffect = effect as IWeaponEffectPickup;
                weaponEffect.OnChangeWeapon(newGunC);
            }
        }
    }



    /// <summary>
    /// iterates through the active effects and checks if any one of them are done
    /// if they are the effect is cleaned out and removed
    /// </summary>
    private void UpdateEffects () {
        foreach (IEffectPickup effect in this.PlayerInventory.ActivePickups.Reverse<IEffectPickup> ()) {
            if (effect.IsEffectFinished ()) {
                effect.Cleanup ();
                this.playerInventory.RemoveEffectPickup (effect);
            }
        }
    }

    // -- unity -- //

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update () {
        UpdateEffects ();
    }

    /// <summary>
    /// Checks for collisions with other objects
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        // If there is a collision with enemy play the damage animation
        if (other.tag == "Enemy") {
            animator.SetTrigger("Damage");
        }
    }
}