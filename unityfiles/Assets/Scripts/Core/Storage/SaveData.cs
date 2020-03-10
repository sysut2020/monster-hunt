using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Simple class which is used in part with "DataHandler" class.
/// All information we want to store, can be kept in this class
/// </summary>
[System.Serializable]
public class SaveData {
    
    private Dictionary<string, int> highScores; 
    private int money;

    public Dictionary<string, int> HighScores { get => HighScores; set => HighScores = value; }
    public int Money { get => Money; set => Money = value; }
}
