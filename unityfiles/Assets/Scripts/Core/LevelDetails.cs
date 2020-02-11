using UnityEngine;


/// <summary>
/// Keeps data about a level
/// </summary>
[CreateAssetMenu(fileName = "Level Details", menuName = "Level details")]
public class LevelDetails : ScriptableObject {
    public int time; // number of seconds the level last
    public int numberOfEnemies;
    public int numberOfLetters;
}