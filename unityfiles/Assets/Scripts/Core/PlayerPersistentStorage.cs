using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDestroyedEventArgs : EventArgs {
    public PlayerInventory PlayerInventoryArgs { get; set; }
}

/// <summary>
/// Used to get persistent information from the players inventory
/// </summary>
public class PlayerPersistentStorage {
    
    private Dictionary<string, int> availableLetters;

    public PlayerPersistentStorage() {
        Player.PlayerDestroyedEvent += CallbackPlayerDestroyedEvent;
    }

    public Dictionary<string, int> AvailableLetters {
        get => availableLetters;
        set => availableLetters = value;
    }

    private int money;
    public int Money {
        get => money;
        set => money = value;
    }

    private void CallbackPlayerDestroyedEvent(object o, PlayerDestroyedEventArgs args) {
        money = args.PlayerInventoryArgs.Money;
        availableLetters = this.formatPlayerInventoryLetter(args.PlayerInventoryArgs.CollectedLetters);
        
    }
    
    /// <summary>
    /// Formats the letters from the players inventory, to a dictionary.
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
    
    

}
