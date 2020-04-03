using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDestroyedEventArgs : EventArgs {
    public PlayerInventory PlayerInventoryArgs { get; set; }
}

/// <summary>
/// Used to handle the game data that needs to be transfered over scenes
/// </summary>
public class GameDataManager {
    private Dictionary<string, int> playerLetters;
    private int money;
    private List<ScoreboardEntry> highScores;
    
    private string highScoreName;
    /// <summary>
    /// Game score holds the score of the current game
    /// </summary>
    private int gameScore;

    public GameDataManager() {
        this.playerLetters = new Dictionary<string, int>();
        this.money = 0;
        this.highScores = new List<ScoreboardEntry>();
        this.gameScore = 0;
        this.highScoreName = "";
        this.LoadData();
    }

    // -- properties -- // 
    public Dictionary<string, int> PlayerLetters {
        get => playerLetters;
    }

    public int GameScore {
        get => gameScore;
    }

    public int Money {
        get => money;
    }

    public List<ScoreboardEntry> HighScores {
        get => highScores;
    }

    public string HighScoreName {
        get => highScoreName;
        set => highScoreName = value;
    }
    // -- events -- // 

    // -- public -- //

    /// <summary>
    /// Adds the provided letter count to the saved letter count
    /// </summary>
    /// <param name="toAdd">the letters to add</param>
    public void AddLetters(Dictionary<string, int> toAdd) {
        if (toAdd != null) {
            foreach (string key in toAdd.Keys) {
                playerLetters[key] = +toAdd[key];
            }
        }
    }

    /// <summary>
    /// Sets the current letter count to the one provided
    /// If null is provided count is set to 0 for every letter
    /// </summary>
    /// <param name="letters">The new letter count</param>
    public void SetLetters(Dictionary<string, int> letters) {
        if (letters != null) {
            this.playerLetters = letters;
        } else {
            this.playerLetters = new Dictionary<string, int>();
        }

    }

    /// <summary>
    /// adds the provided amount to the money count
    /// </summary>
    /// <param name="toAdd">the sum of money to add</param>
    public void AddMoney(int toAdd) {
        this.money += toAdd;
    }

    /// <summary>
    /// Sets the money count to the value provided
    /// </summary>
    /// <param name="m">the new money count</param>
    public void SetMoney(int m) {
        this.money = m;
    }

    /// <summary>
    /// Adds a score to the game score.
    /// </summary>
    /// <param name="score">Score to be added to the totals</param>
    public void AddGameScore(int score) {
        this.gameScore += score;
    }

    /// <summary>
    /// Sets the game score
    /// </summary>
    /// <param name="score">Score to be set</param>
    public void SetGameScore(int score) {
        this.gameScore = score;
    }

    /// <summary>
    /// Saves the currently stored game data
    /// </summary>
    public void SaveData() {
        SaveData saveObj = new SaveData();
        saveObj.Money = this.money;
        saveObj.HighScores = this.highScores;
        saveObj.Score = this.gameScore;
        if (!this.highScoreName.Equals("")) {
            saveObj.Name = this.highScoreName;
            this.highScores.Add(new ScoreboardEntry() {
                PlayerName = this.highScoreName,
                    Score = this.gameScore
            });
            this.gameScore = 0; // reset gameScore when score is added to highScores list
        }
        DataSaver.Save(saveObj);
    }

    // -- private -- // 

    private void LoadData() {
        SaveData saveObj = DataSaver.Load();
        if (saveObj != null) {
            if (saveObj?.Money != 0) {
                this.AddMoney(saveObj.Money);
            }
        }
    }
    
}