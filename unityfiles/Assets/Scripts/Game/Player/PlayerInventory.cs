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
    public int Money { get; set; }

}

/// <summary>
/// container class holding all the player inventory
/// </summary>
public class PlayerInventory {
    private int money;
    private readonly List<String> collectedLetters;


    public PlayerInventory() {
        SubscribeToEvents();
        this.money = 0;
        this.collectedLetters = new List<string>();
    }
    // -- properties -- //
    
    public int Money {
        get => money;
        internal set => this.money = value;
    }

    public Dictionary<string, int> CollectedLetters {
        get => formatPlayerInventoryLetter(collectedLetters);
    }

  
    // -- public -- //

    // -- events -- //
    public event EventHandler<InventoryUpdatedEventArgs> InventoryUpdatedEvent;

    /// <summary>
    /// subscribe from all the events
    /// </summary>
    private void SubscribeToEvents() {
        CoinCollectable.OnCoinCollected += CallbackCoinCollected;
        LetterCollectable.OnLetterCollected += CallbackLetterCollected;
        PowerupCollectable.OnPowerupCollected += CallbackEffectPickup;
        LevelManager.CleanUpEvent += UnsubscribeFromEvents;
    }

    /// <summary>
    /// unsubscribe from all the events
    /// </summary>
    private void UnsubscribeFromEvents(object _, EventArgs __) {
        CoinCollectable.OnCoinCollected -= CallbackCoinCollected;
        LetterCollectable.OnLetterCollected -= CallbackLetterCollected;
        PowerupCollectable.OnPowerupCollected -= CallbackEffectPickup;
        LevelManager.CleanUpEvent -= UnsubscribeFromEvents;
    }
   

    /// <summary>
    /// Coin collected event subscriber function, adds coin amount to invetory
    /// </summary>
    /// <param name="_">object that triggered event</param>
    /// <param name="coin">the coin to add to inventory</param>
    private void CallbackCoinCollected(object _, CoinCollectedArgs coin) {
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

    /// Effect collected event subscriber function, adds effect to inventory
    /// </summary>
    /// <param name="sender">object that triggered event</param>
    /// <param name="effect">the effect to add to inventory</param>
    private void CallbackEffectPickup(object sender, PowerUpCollectedArgs args) {
        throw new NotImplementedException();
    }

    // -- private -- // 

    /// <summary>
    /// Formats the letters in player inventory to the used format
    /// </summary>
    /// <param name="lettersToFormat">The letters we want to format</param>
    /// <returns>Dictionary of letters</returns>
    private Dictionary<string, int> formatPlayerInventoryLetter(List<string> lettersToFormat) {
        Dictionary<string, int> formatedDictionary  = new Dictionary<string, int>();
        foreach (string letter in lettersToFormat){
            if (formatedDictionary.Keys.Contains(letter)){
                formatedDictionary[letter] += 1;
            }
            else{
                formatedDictionary.Add(letter, 1);
            }
        }
        return formatedDictionary;
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
        args.CollectedLetters = collectedLetters;
        args.Money = money;
        InventoryUpdatedEvent?.Invoke(this, args);
    }

}