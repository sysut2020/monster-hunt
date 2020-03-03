using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Handler which saves data on a ".save" file,
/// The handler can save and load data.
/// All of our data we want stored, needs to be saved in a "Save" object.
/// </summary>
[RequireComponent(typeof(PlayerInventory))]
public class DataHandler : MonoBehaviour {
    
    private PlayerInventory playerInventory;
    private string savePath;
    
    private void Start() {
        this.playerInventory = GetComponent<PlayerInventory>();
        //The address path for our save file
        savePath = Application.persistentDataPath + "/gamesave.save";
    }
    
    /// <summary>
    /// Used to save data on a ".save" file
    /// </summary>
    /// <param name="save">The "Save" object we want to save</param>
    public void SaveData(Save save) {
        var binaryFormatter = new BinaryFormatter();
        
        // Using statement makes the file stream automatically close, when instructions are done
        using (var fileStream = File.Create(savePath)) {
            binaryFormatter.Serialize(fileStream, save);
        }
    }
    
    /// <summary>
    /// Used to load the information from a ".save" file
    /// </summary>
    /// <returns>The information to a "Save" object</returns>
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
