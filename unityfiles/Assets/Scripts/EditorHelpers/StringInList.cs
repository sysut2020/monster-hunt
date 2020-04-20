using System;
using UnityEngine;
#if UNITY_EDITOR

#endif

/// <summary>
/// Fills the dropdown list with elements from data/function specified
/// when using the StringInList attribute.
///
/// Usage
///     This will store the string value
///    <code>[StringInList("Cat", "Dog")] public string Animal;</code>
/// 
///    This will store the index of the array value
///    <code>[StringInList("John", "Jack", "Jim")] public int PersonID;</code>
/// 
///    <code>[StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")] public string SceneName;</code>
/// 
/// https://gist.github.com/ProGM/9cb9ae1f7c8c2a4bd3873e4df14a6687
/// </summary>
public class StringInList : PropertyAttribute {
    public delegate string[] GetStringList();

    public StringInList(params string[] list) {
        List = list;
    }

    /// <summary>
    /// Used in conjunction with PropertyDrawersHelper to get method 
    /// for specified type
    /// </summary>
    public StringInList(Type type, string methodName) {
        var method = type.GetMethod(methodName);
        if (method != null) {
            List = method.Invoke(null, null) as string[];
        } else {
            Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + type);
        }
    }

    /// <summary>
    /// Stores list of strings
    /// </summary>
    /// <value>string</value>
    public string[] List {
        get;
        private set;
    }
}