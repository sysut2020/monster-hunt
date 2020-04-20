using UnityEngine;

/// <summary>
/// Containing data about the levels
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
    /// Returns the level time
    /// </summary>
    /// <value>milliseconds</value>
    public int Time { get => time * 1000; }

    /// <summary>
    /// Returns the number of enemies for level
    /// </summary>
    public int NumberOfEnemies { get => numberOfEnemies; }
    /// <summary>
    /// Returns the number of enemies at start of level
    /// </summary>
    public int NumberOfEnemiesAtStart { get => numberOfEnemiesAtStart; }

    /// <summary>
    /// Returns number of letters
    /// </summary>
    public int NumberOfLetters { get => numberOfLetters; }
}