using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
    
    public int SavedMoney;
    public List<string> SavedCollectedLetters;
    public List<IEffectPowerUp> SavedActivePowerUps;
    
}
