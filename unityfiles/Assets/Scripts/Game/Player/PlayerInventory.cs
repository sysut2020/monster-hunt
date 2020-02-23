using System.Runtime.CompilerServices;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


/// <summary>
/// event args for the inventory updated event. 
/// can hold info about the collected letters, the active pickups and current money
/// </summary>
public class InventoryUpdatedEventArgs: EventArgs{
    public List<String> CollectedLetters {get; set;}
    public List<IEffectPowerUp> ActivePickups {get; set;}
    public int Money {get; set;}

}

/// <summary>
/// container class holding all the player inventory
/// </summary>
public class PlayerInventory {
    [SerializeField]
    private int money;

    [SerializeField]
    private List<String> collectedLetters;
    private List<IEffectPowerUp> activePickups;

    

    // -- singelton -- //
    private static PlayerInventory instance;

    public static PlayerInventory Instance{
        get {
            if (instance == null) {
                instance = new PlayerInventory();
            }

            return instance;
        }
    }

    // -- properties -- //
    public int Money {
        get => money;
        internal set => this.money = value;
    }

    public List<String> CollectedLetters {
        get => collectedLetters;
        internal set => this.collectedLetters = value;
    }

    public List<IEffectPowerUp> ActivePickups {
        get => activePickups;
        internal set => this.activePickups = value;
    }
    // -- public -- //

    /// <summary>
    /// Adds the given effect pickup
    /// </summary>
    /// <param name="effect">the pickup to add</param>
    public void AddEffectPickup (IEffectPowerUp effect) {
        this.activePickups.Add (effect);
    }

    /// <summary>
    /// removes the the given effect pickup
    /// </summary>
    /// <param name="effect">the pickup to remove</param>
    public void RemoveEffectPickup (IEffectPowerUp effect) {
        this.activePickups.Remove (effect);
    }

    /// <summary>
    /// adds the provided letter
    /// </summary>
    /// <param name="letter">the letter to add</param>
    public void AddLetter (String letter) {
        this.collectedLetters.Add (letter);
    }

    /// <summary>
    /// adds the provided amount of money
    /// </summary>
    /// <param name="toAdd">the amount of money to add</param>
    public void AddMoney (int toAdd) {
        this.money += toAdd;
    }
    // -- events -- //
    public event EventHandler<InventoryUpdatedEventArgs> InventoryUpdatedEvent;

    private void SubscribeToEvents(){
        PlayerWeaponController.WeaponChangedEvent += c_WeaponChangedEvent;
    }

    private void UnsubscribeFromEvents(){
        PlayerWeaponController.WeaponChangedEvent -= c_WeaponChangedEvent;
    }
    private static void c_WeaponChangedEvent(object o, WeaponChangedEventArgs arg){
        instance.AlertPowerupsOnWeaponChange(arg.NewGunController);
    }


    
    // -- private -- // 


    /// <summary>
    /// Fires an event saying the player inventory has been updated.
    /// to listeners like the UI
    /// </summary>
    private void FireInventoryUpdateEvent(){
        InventoryUpdatedEventArgs args = new InventoryUpdatedEventArgs();
        args.ActivePickups = activePickups;
        args.CollectedLetters = collectedLetters;
        args.Money = money;
        InventoryUpdatedEvent?.Invoke(this, args);
    }

    

    /// <summary>
    /// Alerts the power ups that the weapon has changed
    /// </summary>
    /// <param name="newGunC">The new gun controller to pass to the weapon</param>
    private void AlertPowerupsOnWeaponChange(GunController newGunC) {
        foreach (IEffectPowerUp effect in this.ActivePickups.Reverse<IEffectPowerUp>()) {
            if (effect is IWeaponEffectPowerUp) {
                IWeaponEffectPowerUp weaponEffect = effect as IWeaponEffectPowerUp;
                weaponEffect.OnChangeWeapon(newGunC);
            }
        }
    }

    

}