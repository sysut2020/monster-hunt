using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Handler that can save and load data
/// A locally saved file with ".save" extension
/// All of our data we want stored, needs to be saved in a "Save" object.
/// </summary>
public static class DataSaver {

    private const string savePath = "/GameSaveFile";

    /// <summary>
    /// Used to save a Save data object to file
    /// </summary>
    /// <param name="save">The object to save to file</param>
    public static void Save(SaveData saveData) {
        var binaryFormatter = new BinaryFormatter();

        // Using statement makes the file stream automatically close, when instructions are done
        using(var fileStream = File.Create(Application.persistentDataPath + savePath)) {
            binaryFormatter.Serialize(fileStream, saveData);
        }
    }

    /// <summary>
    /// Used to load a save data object from file
    /// </summary>
    /// <returns>If success, the stored data object is returned. 
    /// Otherwise, null is returned</returns>
    public static SaveData Load() {
        SaveData save = null;

        if (File.Exists(Application.persistentDataPath + savePath)) {
            var binaryFormatter = new BinaryFormatter();

            // Using statement makes the file stream automatically close, when instructions are done
            using(var fileStream = File.Open(Application.persistentDataPath + savePath, FileMode.Open)) {
                save = (SaveData) binaryFormatter.Deserialize(fileStream);
            }
        } else {
            Debug.LogWarning("Save file does not exist");
        }

        return save;
    }
}