using System;
using Object = System.Object;

/// <summary>
/// The scene manager is responsible for loading scenes.
/// The scene manager is a singleton, and there can only exist a single
/// instance of it.
/// It can load a scene by name and its build index.
/// </summary>
public sealed class SceneManager {
    /// <summary>
    /// Scene manager instance
    /// </summary>
    private static SceneManager instance = null;

    /// <summary>
    /// Lock for thread safety
    /// </summary>
    private static readonly object padlock = new Object();

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
    }

    /// <summary>
    /// Has to exist in order to prevent a singleton to be created.
    /// This is to hide the construtor and has to be implemented
    /// </summary>
    private SceneManager() { }

    /// <summary>
    /// Returns the build index of the scene, by name.
    /// </summary>
    /// <param name="sceneName">name of the scene</param>
    /// <returns>build index number</returns>
    public int GetSceneIndexByName(string sceneName) {
        return UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).buildIndex;
    }

    /// <summary>
    /// Returns the scene name from the build index.
    /// </summary>
    /// <param name="index">the scene index</param>
    /// <returns>name of the scene or emp</returns>
    public string GetSceneNameByIndex(int index) {
        return UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(index).name;
    }

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
    /// Throws ArgumentException if index is less than 0
    /// </summary>
    /// <param name="index">the index of the scene</param>
    public void ChangeScene(int index) {
        if (index < 0) {
            throw new ArgumentException("Scene index must be greater or equal to 0");
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