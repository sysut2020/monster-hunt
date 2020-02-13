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
        CollectableEvents.OnCoinCollected += AddMoney;
        CollectableEvents.OnLetterCollected += AddLetter;
        CollectableEvents.OnPowerupCollected += AddEffectPickup;
    }

    private void OnDestroy() {
        CollectableEvents.OnCoinCollected -= AddMoney;
        CollectableEvents.OnLetterCollected -= AddLetter;
        CollectableEvents.OnPowerupCollected -= AddEffectPickup;
    }
    // -- public -- //

    /// <summary>
    /// Adds the given effect pickup
    /// </summary>
    /// <param name="effect">the pickup to add</param>
    public void AddEffectPickup(object sender, PowerUpCollectedArgs effect) {
        this.activePickups.Add(effect.Effect);
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
    public void AddLetter(object sender, LetterCollectedArgs letter) {
        this.collectedLetters.Add(letter.Letter);
    }

    /// <summary>
    /// adds the provided amount of money
    /// </summary>
    /// <param name="toAdd">the amount of money to add</param>
    public void AddMoney(object sender, CoinCollectedArgs coin) {
        this.money += coin.Amount;
    }

    // -- private -- // 

}