using UnityEngine;

/// <summary>
/// Abstract class to represent a collectible item. 
/// Each collectible has a score value
/// </summary>
public abstract class Collectible : MonoBehaviour {
    [SerializeField]
    private protected int scoreValue;

    public int ScoreValue {
        get => scoreValue;
        set => scoreValue = value;
    }
}
