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

    public GameDataManager() {
        this.playerLetters = new Dictionary<string, int>();
        this.money = 0;

        this.LoadData();
    }


    // -- properties -- // 
    public Dictionary<string, int> PlayerLetters {
        get => playerLetters;
    }
    
    public int Money {
        get => money;
    }
    // -- events -- // 

    
    // -- public -- //

    public void AddLetters(Dictionary<string, int> toAdd){
        foreach (string key in toAdd.Keys){
            playerLetters[key] =+ toAdd[key];
        }
    }

    public void SetLetters(Dictionary<string, int> l){
        this.playerLetters = l;
    }

    public void AddMoney(int toAdd){
        this.money += toAdd;
    }

    public void SetMoney(int m){
        this.money = m;
    }

    public void SaveData(){
        SaveData saveObj = new SaveData();
        saveObj.Money = this.money;
        // saveObj.HighScores = steffanos work your magic here
        DataSaver.Save(saveObj);
    }

    // -- private -- // 

    


    private void LoadData(){
        SaveData saveObj = DataSaver.Load();
        if(saveObj != null) {
            if(saveObj?.Money != 0) {this.AddMoney(saveObj.Money);}
            Debug.Log(money);
            //if(saveObj?.HighScores != 0) {steffanos work your magic here;}
        }
        
  
        
    }

    
    
    
    

}
