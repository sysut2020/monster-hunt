using UnityEngine;

public static class CameraUtil {
    /// <summary>
    /// Check if a gameObject is visible for the camera
    /// Returns true if visible, else false.
    /// Found this snippet on:
    /// https://answers.unity.com/questions/8003/how-can-i-know-if-a-gameobject-is-seen-by-a-partic.html?_ga=2.193245293.1336295947.1584236197-1910563228.1584236197
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool IsTargetVisible (Camera camera, GameObject target) {
        var planes = GeometryUtility.CalculateFrustumPlanes (camera);
        var point = target.transform.position;
        foreach (var plane in planes) {
            if (plane.GetDistanceToPoint (point) < 0) {
                return false;
            }
        }

        return true;
    }
}