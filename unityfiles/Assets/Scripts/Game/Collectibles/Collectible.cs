using UnityEngine;

public abstract class Collectible : MonoBehaviour {
    [SerializeField]
    private protected int scoreValue;

    public int ScoreValue {
        get => scoreValue;
        set => scoreValue = value;
    }
}