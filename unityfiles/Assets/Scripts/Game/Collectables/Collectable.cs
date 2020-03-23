using UnityEngine;

public abstract class Collectable : MonoBehaviour {
    [SerializeField]
    private protected int scoreValue;
    
    public abstract string Name { get; }

    public int ScoreValue {
        get => scoreValue;
        set => scoreValue = value;
    }
}