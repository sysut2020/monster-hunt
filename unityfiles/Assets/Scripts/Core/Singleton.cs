using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// http://wiki.unity3d.com/index.php?title=Singleton&_ga=2.56331179.1643914595.1581965468-1068073497.1579017683
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    /// <summary>
    /// Check to see if we're about to be destroyed
    /// </summary>
    private static bool isShuttingDown = false;

    /// <summary>
    /// Thread safety lock
    /// </summary>
    private static object threadLock = new object();

    /// <summary>
    /// The current instance of the object
    /// </summary>
    private static T instance;

    /// <summary>
    /// Access singleton instance through this property.
    /// </summary>
    public static T Instance {
        get {
            if (isShuttingDown) {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }
            // Thread safety
            lock(threadLock) {
                if (instance == null) {
                    instance = (T) FindObjectOfType(typeof(T));
                    if (instance == null) {
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString();
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return instance;
            }
        }
    }

    private void OnApplicationQuit() {
        isShuttingDown = true;
    }

    private void OnDestroy() {

    }
}