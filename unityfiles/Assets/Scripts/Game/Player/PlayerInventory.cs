using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

/// <summary>
/// container class holding all the player inventory
/// </summary>
public class PlayerInventory : MonoBehaviour {
    [SerializeField]
    private int money;

    [SerializeField]
    private List<string> collectedLetters;
    private List<IEffectPickup> activePickups;

    // -- properties -- //
    public int Money {
        get => money;
        internal set => this.money = value;
    }

    public List<string> CollectedLetters {
        get => collectedLetters;
        internal set => this.collectedLetters = value;
    }

    public List<IEffectPickup> ActivePickups {
        get => activePickups;
        internal set => this.activePickups = value;
    }

    private void Start() {
        Coin.OnCoinCollected += OnCoinCollected;
        Letter.OnLetterCollected += OnLetterCollected;
        CollectableEvents.OnPowerupCollected += OnEffectPickup;
    }

    private void OnDestroy() {
        Coin.OnCoinCollected -= OnCoinCollected;
        Letter.OnLetterCollected -= OnLetterCollected;
        CollectableEvents.OnPowerupCollected -= OnEffectPickup;
    }
    // -- public -- //

    /// <summary>
    /// Adds the given effect pickup
    /// </summary>
    /// <param name="effect">the pickup to add</param>
    public void AddEffectPickup(IEffectPickup effect) {
        this.activePickups.Add(effect);
    }

    /// <summary>
    /// Effect collected event subscriber function, adds effect to invetory
    /// </summary>
    /// <param name="sender">object that triggered event</param>
    /// <param name="effect">the effect to add to inventory</param>
    private void OnEffectPickup(object sender, PowerUpCollectedArgs effect) {
        this.AddEffectPickup(effect.Effect);
    }

    /// <summary>
    /// removes the the given effect pickup
    /// </summary>
    /// <param name="effect">the pickup to remove</param>
    public void RemoveEffectPickup(IEffectPickup effect) {
        this.activePickups.Remove(effect);
    }

    /// <summary>
    /// adds the provided letter
    /// </summary>
    /// <param name="letter">the letter to add</param>
    public void AddLetter(string letter) {
        this.collectedLetters.Add(letter);
    }

    /// <summary>
    /// Letter collected subscriber function, adds letter to invetory
    /// </summary>
    /// <param name="sender">object that triggered event</param>
    /// <param name="letter">the letter to add to inventory</param>
    private void OnLetterCollected(object sender, LetterCollectedArgs letter) {
        this.AddLetter(letter.Letter);
    }

    /// <summary>
    /// adds the provided amount of money
    /// </summary>
    /// <param name="toAdd">the amount of money to add</param>
    public void AddMoney(int value) {
        this.money += value;
    }

    /// <summary>
    /// Coin collected event subscriber function, adds coin amount to invetory
    /// </summary>
    /// <param name="sender">object that triggered event</param>
    /// <param name="coin">the coin to add to inventory</param>
    private void OnCoinCollected(object sender, CoinCollectedArgs coin) {
        this.AddMoney(coin.Amount);
    }

    // -- private -- // 

}