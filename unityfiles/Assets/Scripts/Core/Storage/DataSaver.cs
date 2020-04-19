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

public static class DataSaver {

    private const string savePath = "/GameSaveFile";

    /// <summary>
    /// Used to save a save data objet to file
    /// </summary>
    /// <param name="save">The object to save to file</param>
    public static void Save (SaveData saveData) {
        var binaryFormatter = new BinaryFormatter ();

        // Using statement makes the file stream automatically close, when instructions are done
        using (var fileStream = File.Create (Application.persistentDataPath + savePath)) {
            binaryFormatter.Serialize (fileStream, saveData);
        }
    }

    /// <summary>
    /// Used to load a save data objet from file
    /// If unsuccessfully null is returned
    /// </summary>
    /// <returns>The stored save data object</returns>
    public static SaveData Load () {
        SaveData save = null;

        if (File.Exists (Application.persistentDataPath + savePath)) {
            var binaryFormatter = new BinaryFormatter ();

            // Using statement makes the file stream automatically close, when instructions are done
            using (var fileStream = File.Open (Application.persistentDataPath + savePath, FileMode.Open)) {
                save = (SaveData) binaryFormatter.Deserialize (fileStream);
            }

        } else {
            Debug.LogWarning ("Save file does not exist");
        }

        return save;
    }
}