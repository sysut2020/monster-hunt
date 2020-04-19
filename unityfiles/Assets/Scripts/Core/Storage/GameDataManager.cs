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
    /// Adds a new high score entry with current game score, and the provided 
    /// name. The game score is reset when entry is added.
    /// </summary>
    /// <param name="name">name of the score enrty (player name)</param>
    public void AddNewHighScoreEntry(string name) {

        this.highScores.Add(new ScoreboardEntry() {
            PlayerName = name,
                Score = this.gameScore
        });

        this.gameScore = 0; // reset gameScore when score is added to highScores list
    }

    /// <summary>
    /// Sets the game high scores
    /// </summary>
    /// <param name="score">the new highscore list</param>
    public void SetHighScores(List<ScoreboardEntry> score) {
        this.highScores = score;
    }

    /// <summary>
    /// Saves the currently stored game data
    /// </summary>
    public void SaveData() {
        DataSaver.Save(new SaveData {
            Money = this.money,
                HighScores = this.highScores,
        });
    }

    private void LoadData() {
        SaveData saveObj = DataSaver.Load();
        if (saveObj != null) {
            if (saveObj?.Money != 0) {
                this.AddMoney(saveObj.Money);
            }

            if (saveObj?.HighScores != null) {
                this.SetHighScores(saveObj.HighScores);
            }
        }
    }

}