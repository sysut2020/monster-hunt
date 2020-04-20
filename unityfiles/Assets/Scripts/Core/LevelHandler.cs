using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the changing of levels in the game.
/// It can start a specific scene/level or go to the next level.
/// It toggles between lettergame, the different levels, and finally the scoreboard
/// scene when there are no more levels left.
/// </summary>
public class LevelHandler : MonoBehaviour {

    /// <summary>
    /// All the levels the player is to play through
    /// </summary>
    [SerializeField]
    [StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")]
    private string[] huntingGameSceneNames;

    [SerializeField]
    [StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")]
    private string scoreboardSceneName;

    [SerializeField]
    [StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")]
    private string letterGameSceneName;

    /// <summary>
    /// Index of the current scene. If it is a hunting game it is >=0 else its -1
    /// </summary>
    /// <value>scene index</value>
    private static int CurrentScene { get; set; } = -1;

    /// <summary>
    /// Index of the last scene. If it was a hunting game it is >=0 else its -1
    /// </summary>
    /// <value>scene index</value>
    private static int LastHuntingScene { get; set; } = -1;

    private static bool isCreated;

    private void Awake() {
        if (isCreated) { return; }
        isCreated = true;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Changes to the next level. 
    ///     - If we are in letter game, go to the next
    ///     hunting game level. 
    ///     - If we are in the letter game and there are no more levels
    ///     the game is finished, and scoreboard scene is loaded.
    /// </summary>
    public void NextLevel() {
        string nextLevelName;
        if (CurrentScene >= 0) {
            LastHuntingScene = CurrentScene;
            nextLevelName = this.letterGameSceneName;
        } else {
            int nameIndex = LastHuntingScene >= 0 ? LastHuntingScene + 1 : 0;
            if (nameIndex <= this.huntingGameSceneNames.Length - 1) {
                nextLevelName = this.huntingGameSceneNames[nameIndex];
            } else {
                nextLevelName = this.scoreboardSceneName;
            }
        }
        this.PlayLevel(nextLevelName);
    }

    /// <summary>
    /// Returns a hunting scene local array index from the array of levels by name.
    /// </summary>
    /// <param name="name">name of the scene to find</param>
    /// <returns>array index of the scene in the list or -1 if not found</returns>
    private int GetHuntingSceneIndexByName(string name) {
        for (int i = 0; i < this.huntingGameSceneNames.Length; i++) {
            var sceneName = this.huntingGameSceneNames[i];
            if (sceneName == name) {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Load a scene by its name, and set current scene to that level.
    /// </summary>
    /// <param name="name">name of the scene</param>
    public void PlayLevel(string name) {
        SceneManager.Instance.ChangeScene(name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        CurrentScene = GetHuntingSceneIndexByName(scene.name);
    }

}