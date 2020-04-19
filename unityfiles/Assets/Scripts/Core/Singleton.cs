using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// http://wiki.unity3d.com/index.php?title=Singleton&_ga=2.56331179.1643914595.1581965468-1068073497.1579017683
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    // Check to see if we're about to be destroyed.
    private static bool isShuttingDown = false;

    // Thread safety lock
    private static object threadLock = new object ();

    // Current instance of the object
    private static T instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance {
        get {
            if (isShuttingDown) {
                Debug.LogWarning ("[Singleton] Instance '" + typeof (T) +
                    "' already destroyed. Returning null.");
                return null;
            }
            // Thread safety
            lock (threadLock) {
                if (instance == null) {
                    instance = (T) FindObjectOfType (typeof (T));
                    if (instance == null) {
                        var singletonObject = new GameObject ();
                        instance = singletonObject.AddComponent<T> ();
                        singletonObject.name = typeof (T).ToString ();
                        DontDestroyOnLoad (singletonObject);
                    }
                }
                return instance;
            }
        }
    }

    private void OnApplicationQuit () {
        isShuttingDown = true;
    }

    private void OnDestroy () {

    }
}