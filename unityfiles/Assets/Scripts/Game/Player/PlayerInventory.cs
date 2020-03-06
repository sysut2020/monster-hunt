using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

/// <summary>
/// event args for the inventory updated event. 
/// can hold info about the collected letters, the active pickups and current money
/// </summary>
public class InventoryUpdatedEventArgs : EventArgs {
    public List<String> CollectedLetters { get; set; }
    public List<IEffectPowerUp> ActivePickups { get; set; }
    public int Money { get; set; }

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

    // -- properties -- //
    public PlayerInventory() {
        SubscribeToEvents();
        this.money = 0;
        this.collectedLetters = new List<string>();
        this.activePickups = new List<IEffectPowerUp>();
    }

    public int Money {
        get => money;
        internal set => this.money = value;
    }

    public List<string> CollectedLetters {
        get => collectedLetters;
        internal set => this.collectedLetters = value;
    }

    public List<IEffectPowerUp> ActivePickups {
        get => activePickups;
        internal set => this.activePickups = value;
    }

    // -- public -- //

    // -- events -- //
    public event EventHandler<InventoryUpdatedEventArgs> InventoryUpdatedEvent;

    /// <summary>
    /// subscribe from all the events
    /// </summary>
    private void SubscribeToEvents() {
        PlayerWeaponController.WeaponChangedEvent += CallbackWeaponChangedEvent;
        CoinCollectable.OnCoinCollected += CallbackCoinCollected;
        LetterCollectable.OnLetterCollected += CallbackLetterCollected;
        PowerupCollectable.OnPowerupCollected += CallbackEffectPickup;
        LevelManager.CleanUpEvent += UnsubscribeFromEvents;
    }

    /// <summary>
    /// unsubscribe from all the events
    /// </summary>
    private void UnsubscribeFromEvents(object _, EventArgs __) {
        PlayerWeaponController.WeaponChangedEvent -= CallbackWeaponChangedEvent;
        CoinCollectable.OnCoinCollected -= CallbackCoinCollected;
        LetterCollectable.OnLetterCollected -= CallbackLetterCollected;
        PowerupCollectable.OnPowerupCollected -= CallbackEffectPickup;
        LevelManager.CleanUpEvent -= UnsubscribeFromEvents;
    }
    private void CallbackWeaponChangedEvent(object o, WeaponChangedEventArgs arg) {
        AlertPowerupsOnWeaponChange(arg.NewGunController);
    }

    /// <summary>
    /// Coin collected event subscriber function, adds coin amount to invetory
    /// </summary>
    /// <param name="sender">object that triggered event</param>
    /// <param name="coin">the coin to add to inventory</param>
    private void CallbackCoinCollected(object sender, CoinCollectedArgs coin) {
        this.AddMoney(coin.Amount);
    }

    /// <summary>
    /// Letter collected subscriber function, adds letter to invetory
    /// </summary>
    /// <param name="sender">object that triggered event</param>
    /// <param name="letter">the letter to add to inventory</param>
    private void CallbackLetterCollected(object sender, LetterCollectedArgs letter) {
        this.AddLetter(letter.Letter);
    }

    /// Effect collected event subscriber function, adds effect to invetory
    /// </summary>
    /// <param name="sender">object that triggered event</param>
    /// <param name="effect">the effect to add to inventory</param>
    private void CallbackEffectPickup(object sender, PowerUpCollectedArgs args) {
        throw new NotImplementedException();
    }

    // -- private -- // 

    /// <summary>
    /// Adds the given effect pickup
    /// </summary>
    /// <param name="effect">the pickup to add</param>
    /// <summary>
    public void AddEffectPickup(IEffectPowerUp effect) {
        this.activePickups.Add(effect);
    }

    /// <summary>
    /// removes the the given effect pickup
    /// </summary>
    /// /// <param name="effect">the pickup to remove</param>
    public void RemoveEffectPickup(IEffectPowerUp effect) {
        this.activePickups.Remove(effect);
    }

    /// <summary>
    /// adds the provided letter
    /// </summary>
    /// <param name="letter">the letter to add</param>
    private void AddLetter(string letter) {
        this.collectedLetters.Add(letter);
    }

    /// <summary>
    /// adds the provided amount of money
    /// </summary>
    /// <param name="toAdd">the amount of money to add</param>
    private void AddMoney(int value) {
        this.money += value;
    }

    /// <summary>
    /// Fires an event saying the player inventory has been updated.
    /// to listeners like the UI
    /// </summary>
    private void FireInventoryUpdateEvent() {
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