using UnityEngine;

public class SingletonTEST : Singleton<SingletonTEST> {
	// Prevents from creating : new SingletonTETS();
	protected SingletonTEST() { }

	private void Start() {
		Debug.Log(" i am started");
	}

}