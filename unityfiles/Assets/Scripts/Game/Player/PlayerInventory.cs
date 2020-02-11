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
    private List<Letter> collectedLetters;
    private List<IEffectPickup> activePickups;

    // -- properties -- //
    public int Money {
        get => money;
        internal set => this.money = value;
    }

    public List<Letter> CollectedLetters {
        get => collectedLetters;
        internal set => this.collectedLetters = value;
    }

    public List<IEffectPickup> ActivePickups {
        get => activePickups;
        internal set => this.activePickups = value;
    }

    private void Start() {
        CollectableEvents.onCoinPickup += AddMoney;
        CollectableEvents.onLetterPickup += AddLetter;
        CollectableEvents.onPowerupPickup += AddEffectPickup;
    }

    private void OnDestroy() {
        CollectableEvents.onCoinPickup -= AddMoney;
        CollectableEvents.onLetterPickup -= AddLetter;
        CollectableEvents.onPowerupPickup -= AddEffectPickup;
    }
    // -- public -- //

    /// <summary>
    /// Adds the given effect pickup
    /// </summary>
    /// <param name="effect">the pickup to add</param>
    public void AddEffectPickup (IEffectPickup effect) {
        this.activePickups.Add (effect);
    }

    /// <summary>
    /// removes the the given effect pickup
    /// </summary>
    /// <param name="effect">the pickup to remove</param>
    public void RemoveEffectPickup (IEffectPickup effect) {
        this.activePickups.Remove (effect);
    }

    /// <summary>
    /// adds the provided letter
    /// </summary>
    /// <param name="letter">the letter to add</param>
    public void AddLetter (Letter letter) {
        this.collectedLetters.Add(letter);
    }

    /// <summary>
    /// adds the provided amount of money
    /// </summary>
    /// <param name="toAdd">the amount of money to add</param>
    public void AddMoney (int value) {
        this.money += value;
    }

    // -- private -- // 

}