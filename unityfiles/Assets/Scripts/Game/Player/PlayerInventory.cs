using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Container class holding all the player inventory
/// </summary>
public class PlayerInventory {
    private int money;
    private readonly List<string> collectedLetters;

    public PlayerInventory() {
        SubscribeToEvents();
        this.money = 0;
        this.collectedLetters = new List<string>();
    }

    public int Money {
        get => money;
        internal set => this.money = value;
    }

    public Dictionary<string, int> CollectedLetters {
        get => formatPlayerInventoryLetter(collectedLetters);
    }
    
    /// <summary>
    /// Subscribe to all the events
    /// </summary>
    private void SubscribeToEvents() {
        LetterCollectible.OnLetterCollected += CallbackLetterCollected;
        CoinCollectible.OnCoinCollected += CallbackCoinCollected;
        HuntingLevelController.CleanUpEvent += UnsubscribeFromEvents;
    }

    /// <summary>
    /// Unsubscribe from all the events
    /// </summary>
    private void UnsubscribeFromEvents(object _, EventArgs __) {
        LetterCollectible.OnLetterCollected -= CallbackLetterCollected;
        CoinCollectible.OnCoinCollected -= CallbackCoinCollected;
        HuntingLevelController.CleanUpEvent -= UnsubscribeFromEvents;
    }

    /// <summary>
    /// Coin collected event subscriber function, adds coin amount to inventory
    /// </summary>
    /// <param name="_">object that triggered event</param>
    /// <param name="coin">the coin to add to inventory</param>
    private void CallbackCoinCollected(object _, CoinCollectedArgs coin) {
        this.AddMoney(coin.Amount);
    }

    /// <summary>
    /// Letter collected subscriber function, adds letter to inventory
    /// </summary>
    /// <param name="sender">object that triggered event</param>
    /// <param name="letter">the letter to add to inventory</param>
    private void CallbackLetterCollected(object sender, LetterCollectedArgs letter) {
        this.AddLetter(letter.Letter);
    }

    /// <summary>
    /// Formats the letters in player inventory to the used format
    /// </summary>
    /// <param name="lettersToFormat">The letters we want to format</param>
    /// <returns>Dictionary of letters</returns>
    private Dictionary<string, int> formatPlayerInventoryLetter(List<string> lettersToFormat) {
        Dictionary<string, int> formattedDictionary = new Dictionary<string, int>();
        foreach (string letter in lettersToFormat) {
            if (formattedDictionary.Keys.Contains(letter)) {
                formattedDictionary[letter] += 1;
            } else {
                formattedDictionary.Add(letter, 1);
            }
        }

        return formattedDictionary;
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
    /// <param name="value">the amount of money to add</param>
    private void AddMoney(int value) {
        this.money += value;
    }
}