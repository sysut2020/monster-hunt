using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Can be attached to GameObjects by script to create callbacks on instantiated
/// GameObject when they are destroyed.
/// </summary>
public class DestroyedCallbackNotifyer : MonoBehaviour {

	private bool isApplicationQuitting = false;
	private Action callback;

	/// <summary>
	/// Sets the method to be called when the GameObject is destroyed.
	/// </summary>
	/// <param name="callback"></param>
	public void SetCallback(Action callback) {
		if (callback == null) Debug.LogError("A callback was not provided");
		this.callback = callback;

	}

	/// <summary>
	/// Set flag that the application is quiting.
	/// This method is executed by UnityEngine when application is about to quit.
	/// </summary>
	private void OnApplicationQuit() {
		isApplicationQuitting = true;
	}

	/// <summary>
	/// Invokes the callback if the application is not quitting.
	/// </summary>
	private void OnDestroy() {
		if (isApplicationQuitting) { return; }
		this.callback?.Invoke();
		this.callback = null;
	}
}