using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// Simple class which is used in part with "DataHandler" class.
/// All information we want to store, can be kept in this class
/// </summary>
[System.Serializable]
public class Save {
    
    public int SavedMoney; 
    public Dictionary<string, int> availableLetters;

    private int score;

    public int Score {
        get => score;
        set => score = value;
    }
}
