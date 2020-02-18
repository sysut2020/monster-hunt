using System;
using UnityEngine;

public abstract class Collectable : MonoBehaviour {

	/// <summary>
	/// Rect transform of the GUI target we want to target.
	/// </summary>
	private RectTransform worldGuiTarget;

	/// <summary>
	/// Game object that moves the target position in world space as the camera
	/// moves, which is the target we aim for.
	/// </summary>
	private GameObject guiWorldPositionHelper;

	protected void FindPosition<T>() {
		System.Type av = typeof(T);
		MonoBehaviour guiTarget = (MonoBehaviour) UnityEngine.Object.FindObjectOfType(av);
		if (guiTarget != null) {
			guiTarget.TryGetComponent(out worldGuiTarget);
			guiWorldPositionHelper = new GameObject("GUI WORLD POSITION");
		} else {
			Debug.LogWarning("GUI TARGET DO NOT EXIST IN THE SCENE.");
		}
	}

	/// <summary>
	/// Calculates the position of the collectible to the GUI target, when camera
	/// moves.
	/// </summary>
	private void CalculatePosition() {
		Vector3 resultPosition;
		Vector2 vectorRectTransformPosition = worldGuiTarget.transform.position;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(
			worldGuiTarget, vectorRectTransformPosition, FindObjectOfType<Camera>(), out resultPosition
		);
		guiWorldPositionHelper.transform.position = resultPosition;
	}

	/// <summary>
	/// Moves this gameobject towards the GUI target.
	/// </summary>
	private void MoveToTarget() {
		if (guiWorldPositionHelper != null) {
			CalculatePosition();
			Vector3 newPosition = Vector3.MoveTowards(this.transform.position, guiWorldPositionHelper.transform.position, 0.5f);
			if (Vector2.Distance(this.transform.position, guiWorldPositionHelper.transform.position) < 0.5f) {
				Destroy(guiWorldPositionHelper);
				Destroy(this.gameObject);
			} else {
				this.transform.position = newPosition;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	private void FixedUpdate() {
		this.MoveToTarget();
	}
}