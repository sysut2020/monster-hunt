using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Exposes all scenes in the build index by name as dropdown menu
/// in the editor, for easy selection of the scene to change to.
/// Button clicked event is added on creation, changes scene when button is clicked.
/// </summary>
[RequireComponent(typeof(Button))]
public class SceneChangerButton : MonoBehaviour {

    [SerializeField]
    [StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")]
    private string sceneName;

    private Button sceneChangeButton;

    private void Awake() {
        sceneChangeButton = GetComponent<Button>();
    }
    private void Start() {
        sceneChangeButton.onClick.AddListener(OnChangeSceneClicked);
    }

    private void OnDestroy() {
        sceneChangeButton.onClick.RemoveAllListeners();
    }

    void OnChangeSceneClicked() {
        SceneManager.Instance.ChangeScene(sceneName);
    }
}