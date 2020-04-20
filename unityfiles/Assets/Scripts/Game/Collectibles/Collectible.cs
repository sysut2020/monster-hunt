using UnityEngine;

/// <summary>
/// Used to give a score value to each collectible item
/// </summary>
public abstract class Collectible : MonoBehaviour {
    [SerializeField]
    private protected int scoreValue;

    public int ScoreValue {
        get => scoreValue;
        set => scoreValue = value;
    }
}