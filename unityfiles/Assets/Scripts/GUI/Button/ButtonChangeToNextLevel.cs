using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Changes to next level
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