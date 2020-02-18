using UnityEngine;


/// <summary>
/// Keeps data about a level
/// </summary>
[CreateAssetMenu(fileName = "Level Details", menuName = "Level details")]
public class LevelDetails : ScriptableObject {

    [Tooltip ("number of seconds the level should last")]
    [SerializeField]
    private int time;

    [Tooltip ("number of enemies to killed before level complete")]
    [SerializeField]
    private int numberOfEnemies;

    [Tooltip ("number of letters to be spawned during game play")]
    [SerializeField]
    private int numberOfLetters;

    public int Time { get => time;}
    public int NumberOfEnemies { get => numberOfEnemies;}
    public int NumberOfLetters { get => numberOfLetters;}
}