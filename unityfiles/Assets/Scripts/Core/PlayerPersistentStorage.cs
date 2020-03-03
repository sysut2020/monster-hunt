using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDestroyedEventArgs : EventArgs {
    public PlayerInventory PlayerInventoryArgs { get; set; }
}

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

    private Dictionary<string, int> formatPlayerInventoryLetter(List<string> lettersToFormat) {
        Dictionary<string, int> ret  = new Dictionary<string, int>();
        foreach (string letter in lettersToFormat){
            if (ret.Keys.Contains(letter)){
                ret[letter] += 1;
            }
            else{
                ret.Add(letter, 1);
            }
        }

        return ret;
    }
    
    

}
