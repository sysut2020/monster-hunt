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

    private DataHandler instance = null;

    private DataHandler() { }
    
    private void Start() {
        this.playerInventory = GetComponent<PlayerInventory>();
        savePath = Application.persistentDataPath + "/gamesave.save";
    }

    public void SaveData(Save save) {
        
        
        var binaryFormatter = new BinaryFormatter();
        
        // Using statement makes the file stream automatically close, when instructions are done
        using (var fileStream = File.Create(savePath)) {
            binaryFormatter.Serialize(fileStream, save);
        }
    }

    public Save LoadData() {
        Save save = null;
        if (File.Exists(savePath)){
            
            
            var binaryFormatter = new BinaryFormatter();
            
            // Using statement makes the file stream automatically close, when instructions are done
            using (var fileStream = File.Open(savePath, FileMode.Open)){
                save = (Save) binaryFormatter.Deserialize(fileStream);
            }
            
        }
        else {
            Debug.LogWarning("Save file does not exist");
        }

        return save;
    }
}
