using UnityEngine;

/// <summary>
/// Keeps data about a level
/// </summary>
[CreateAssetMenu(fileName = "Level Details", menuName = "Level details")]
public class LevelDetails : ScriptableObject {

    [Tooltip("number of seconds the level should last")]
    [SerializeField]
    private int time;

    [Tooltip("number of enemies to killed before level complete")]
    [SerializeField]
    private int numberOfEnemies;

    [Tooltip("number of enemies spawned at start")]
    [SerializeField]
    private int numberOfEnemiesAtStart = 5;

    [Tooltip("number of letters to be spawned during game play")]
    [SerializeField]
    private int numberOfLetters;

    /// <summary>
    /// Returns the level time in milliseconds
    /// </summary>
    public int Time { get => time * 1000; }
    public int NumberOfEnemies { get => numberOfEnemies; }
    public int NumberOfEnemiesAtStart { get => numberOfEnemiesAtStart; }
    public int NumberOfLetters { get => numberOfLetters; }
}