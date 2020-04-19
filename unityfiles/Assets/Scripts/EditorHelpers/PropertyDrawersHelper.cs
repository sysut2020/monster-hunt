using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Helper class for PropertyDrawer that can define functions
/// which can be used to generate outputs for the property drawers.
/// 
/// Like getting all the active scene in build settings.
/// 
/// https://gist.github.com/ProGM/9cb9ae1f7c8c2a4bd3873e4df14a6687
/// </summary>
public static class PropertyDrawersHelper {
#if UNITY_EDITOR

    // [StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")] private string SceneName;
    public static string[] AllSceneNames() {
        var temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
            if (S.enabled) {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                temp.Add(name);
            }
        }
        return temp.ToArray();
    }

#endif
}