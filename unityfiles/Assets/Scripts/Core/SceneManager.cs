using System;

/// <summary>
/// The scene manager is responsible for loading scenes.
/// The scene manager is a singleton, and there can only exist a single
/// instane of it.
/// It can load a scene by name and its build index.
/// </summary>
public sealed class SceneManager {
    // Scenemanager instance
    private static SceneManager instance = null;
    // Lock for thread safety
    private static readonly object padlock = new Object();

    private DataHandler dataHandler;

    /// <summary>
    /// Check if instance of SceneManager exist, if it does, return it.
    /// Else lock the resource and create a new instance of it and return it.
    /// </summary>
    /// <value>SceneManager instance</value>
    public static SceneManager Instance {
        get {
            {
                lock(padlock) {
                    if (instance == null) {
                        instance = new SceneManager();
                    }
                    return instance;
                }
            }
        }
        private set { }
    }

    private SceneManager() { }

    /// <summary>
    /// Loads a new scene by its scene name
    /// Throws ArgumentNullException if the name is NULL
    /// </summary>
    /// <param name="sceneName">the name of the scene</param>
    public void ChangeScene(string sceneName) {
        if (sceneName == null) {
            throw new ArgumentNullException("Scene name can not be null");
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Loads a new scene by its build index
    /// Throws ArgumentException index is smaller then 0
    /// </summary>
    /// <param name="index">the index of the scene</param>
    public void ChangeScene(int index) {
        dataHandler.SaveData();
        
        if (index < 0) {
            throw new ArgumentException("Scene index must be >= 0");
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    /// <summary>
    /// Reloads the currently active scene
    /// </summary>
    public void RestartCurrentScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

}