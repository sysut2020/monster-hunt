using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class DataHandler : MonoBehaviour {

    private PlayerInventory playerInventory;
    private string savePath;

    private void Start() {
        this.playerInventory = GetComponent<PlayerInventory>();
        savePath = Application.persistentDataPath + "/gamesave.save";
    }

    public void SaveData() {
        var save = new Save() {
            SavedMoney = playerInventory.Money,
            SavedCollectedLetters = playerInventory.CollectedLetters,
            SavedActivePowerUps = playerInventory.ActivePickups
        };
        
        var binaryFormatter = new BinaryFormatter();
        
        // Using statement makes the file stream automatically close, when instructions are done
        using (var fileStream = File.Create(savePath)) {
            binaryFormatter.Serialize(fileStream, save);
        }
    }

    public void LoadData() {
        if (File.Exists(savePath)){
            Save save;
            
            var binaryFormatter = new BinaryFormatter();
            
            // Using statement makes the file stream automatically close, when instructions are done
            using (var fileStream = File.Open(savePath, FileMode.Open)){
                save = (Save) binaryFormatter.Deserialize(fileStream);
            }

            playerInventory.Money = save.SavedMoney;
            playerInventory.CollectedLetters = save.SavedCollectedLetters;
            playerInventory.ActivePickups = save.SavedActivePowerUps;
        }
        else {
            Debug.LogWarning("Save file does not exist");
        }
    }
}
