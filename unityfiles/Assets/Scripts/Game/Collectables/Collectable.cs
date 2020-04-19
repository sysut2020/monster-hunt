using UnityEngine;

public abstract class Collectable : MonoBehaviour {
	[SerializeField]
	private protected int scoreValue;

	public int ScoreValue {
		get => scoreValue;
		set => scoreValue = value;
	}
}