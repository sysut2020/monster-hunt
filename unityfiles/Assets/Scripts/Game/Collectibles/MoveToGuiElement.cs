using UnityEngine;

/// <summary>
/// Used to move objects from game position, to where on the camera the desired GUI element is being displayed
/// </summary>
public class MoveToGuiElement : MonoBehaviour {

    /// <summary>
    /// The speed it will use to fly to the location
    /// </summary>
    [SerializeField]
    [Range(0, 2)]
    private float speed;

    [SerializeField]
    [Tooltip("The maximum distance between the target and this GO at which it should destroy itself")]
    private float selfDestroyDistance;

    /// <summary>
    /// Rect transform of the GUI target we want to target.
    /// </summary>
    private RectTransform worldGuiTarget;

    /// <summary>
    /// Game object that moves the target position in world space as the camera
    /// moves, which is the target we aim for.
    /// </summary>
    private GameObject guiWorldPositionHelper;

    /// <summary>
    /// Finds the GUI target by class; it selects the first target of the type.
    /// So if there are multiple objects with the same type, it will select the first 
    /// it finds.
    /// </summary>
    /// <typeparam name="T">The class type to search for</typeparam>
    public void FindTarget<T>() {
        System.Type av = typeof(T);
        MonoBehaviour guiTarget = (MonoBehaviour)UnityEngine.Object.FindObjectOfType(av);
        if (guiTarget != null) {
            guiTarget.TryGetComponent(out worldGuiTarget);
            guiWorldPositionHelper = new GameObject("GUI TARGET WORLD POSITION");
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
            Vector3 newPosition = Vector3.MoveTowards(this.transform.position, guiWorldPositionHelper.transform.position, speed);
            if (Vector2.Distance(this.transform.position, guiWorldPositionHelper.transform.position) < selfDestroyDistance) {
                Destroy(guiWorldPositionHelper);
                Destroy(this.gameObject);
            } else {
                this.transform.position = newPosition;
            }
        }
    }

    private void FixedUpdate() {
        this.MoveToTarget();
    }

}