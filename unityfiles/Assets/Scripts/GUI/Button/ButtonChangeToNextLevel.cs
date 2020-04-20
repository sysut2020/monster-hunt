using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Makes a button change to next level
/// Attache script to a button to make it change to next level
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonChangeToNextLevel : MonoBehaviour {

    private Button button;
    private LevelHandler levelHandler;

    private void Awake() {
        this.button = GetComponent<Button>();
        levelHandler = FindObjectOfType<LevelHandler>();
        button.onClick.AddListener(() => this.levelHandler?.NextLevel());
    }

    private void OnDestroy() {
        button.onClick.RemoveAllListeners();
    }
}