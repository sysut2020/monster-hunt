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
public class GameDataManager {
    private Dictionary<string, int> playerLetters;
    private int money;
    private List<ScoreboardEntry> highScores;

    public GameDataManager() {
        this.playerLetters = new Dictionary<string, int>();
        this.money = 0;
        this.highScores = new List<ScoreboardEntry>();

        this.LoadData();
    }


    // -- properties -- // 
    public Dictionary<string, int> PlayerLetters {
        get => playerLetters;
    }

    public int Money {
        get => money;
    }

    public List<ScoreboardEntry> HighScores {
        get => highScores;
    }
    // -- events -- // 


    // -- public -- //

    public void AddLetters(Dictionary<string, int> toAdd) {
        foreach (string key in toAdd.Keys) {
            playerLetters[key] = +toAdd[key];
        }
    }

    public void SetLetters(Dictionary<string, int> l) {
        this.playerLetters = l;
    }

    public void AddMoney(int toAdd) {
        this.money += toAdd;
    }

    public void SetMoney(int m) {
        this.money = m;
    }

    private void AddNewHighScore(string name, int score) {
        highScores.Add(new ScoreboardEntry(name, score));
    }

    public void SetHighScores(List<ScoreboardEntry> highScores) {
        this.highScores = highScores;
    }

    public void SaveData() {
        SaveData saveObj = new SaveData();
        saveObj.Money = this.money;
        saveObj.HighScores = this.highScores;
        DataSaver.Save(saveObj);
    }

    // -- private -- // 


    private void LoadData() {
        SaveData saveObj = DataSaver.Load();
        if (saveObj != null) {
            if (saveObj?.Money != 0) {
                this.AddMoney(saveObj.Money);
            }
            
            if (saveObj?.HighScores != null) {
                SetHighScores(saveObj.HighScores);
            }

            // foreach (var entry in HighScores) {
            //     Debug.Log("entry: " + entry.Name);
            // }
        }
    }
}