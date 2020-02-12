using UnityEngine;


/// <summary>
/// Keeps data about a level
/// </summary>
[CreateAssetMenu(fileName = "Level Details", menuName = "Level details")]
public class LevelDetails : ScriptableObject {
    [SerializeField]
    private int time; // number of seconds the level should last

    [SerializeField]
    private int numberOfEnemies;

    [SerializeField]
    private int numberOfLetters; // number of letters to be spawned during game play

    public int Time => time;

    public int NumberOfEnemies => numberOfEnemies;

    public int NumberOfLetters => numberOfLetters;
}