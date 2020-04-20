using UnityEditor;
using UnityEngine;

/// <summary>
/// Camera follow is responsible for moving a camera 
/// towards a target. The camera follow can follow any GameObject.
/// A speed modifier can be adjusted to set the movement smoothnes speed 
/// of the camera, where a lower value result in a movement delay of the camera.
/// 
/// A boundry can be set so that the camera doesnt travel outside the game world
/// when a player is at an edge.
/// 
/// In editor mode when the object which has this class assigned will show
/// two lines in the editor window, which represents the left and right 
/// clamps.The cameras center point will not go past these points.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private Transform targetToFollow;

    /// <summary>
    /// Adjusts how smooth/slowly the camera will follow the target.
    /// </summary>
    [SerializeField]
    [Range(0, 1)]
    private float followSmoothnes;

    // SmoothDamp velocity
    private Vector3 smoothVelocity = Vector3.zero;

    [SerializeField]
    [Tooltip("Position on the top of the scene where the camera should stop.")]
    private float topEdgeClamp = 1;

    [SerializeField]
    [Tooltip("Position on the right side of the scene where the camera should stop.")]
    private float rightEdgeClamp = 1;

    [SerializeField]
    [Tooltip("Position on the bottom of the scene where the camera should stop.")]
    private float bottomEdgeClamp = -1;

    [SerializeField]
    [Tooltip("Position on the left side of the scene where the camera should stop.")]
    private float leftEdgeClamp = -1;

    /// <summary>
    /// Moves the camera to the target position with smooth damp,
    /// the X position is limited to left/right clamp.
    /// 
    /// Using LateUpdate since the player character is moved in Update; 
    /// and we now have the new position for the current frame
    /// </summary>
    private void LateUpdate() {
        Vector3 currentTargetPosition = targetToFollow.position;
        // Make sure we dont move the camera in Z position.
        currentTargetPosition.z = this.transform.position.z;

        // Dont use Time.deltatime here, 
        // it is automatically calculated in the function
        Vector3 newPosition = Vector3.SmoothDamp(
            transform.position,
            currentTargetPosition,
            ref smoothVelocity,
            followSmoothnes);

        this.transform.position = new Vector3(
            Mathf.Clamp(newPosition.x, this.leftEdgeClamp, this.rightEdgeClamp),
            Mathf.Clamp(newPosition.y, this.bottomEdgeClamp, this.topEdgeClamp),
            newPosition.z);
    }

    /// <summary>
    /// Draws the left and right clamp in editor window, when the 
    /// object is selected.
    /// </summary>
#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        float lineTop = 10 + this.transform.position.y;
        float lineBottom = -10 + this.transform.position.y;

        Handles.color = Color.magenta;
        Handles.DrawLine(new Vector3(lineTop, this.topEdgeClamp, 0), new Vector3(lineBottom, this.topEdgeClamp, 0));
        Handles.Label(new Vector3(this.transform.position.x, this.topEdgeClamp, 0), "TOP");

        Handles.DrawLine(new Vector3(this.rightEdgeClamp, lineTop, 0), new Vector3(this.rightEdgeClamp, lineBottom, 0));
        Handles.Label(new Vector3(this.rightEdgeClamp, this.transform.position.y, 0), "RIGHT");

        Handles.DrawLine(new Vector3(lineTop, this.bottomEdgeClamp, 0), new Vector3(lineBottom, this.bottomEdgeClamp, 0));
        Handles.Label(new Vector3(this.transform.position.x, this.bottomEdgeClamp, 0), "BOTTOM");

        Handles.DrawLine(new Vector3(this.leftEdgeClamp, lineTop, 0), new Vector3(this.leftEdgeClamp, lineBottom, 0));
        Handles.Label(new Vector3(this.leftEdgeClamp, this.transform.position.y, 0), "LEFT");

    }
#endif
}