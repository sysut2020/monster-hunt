using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Set the game state on button click to the state set in the inspector
/// in Unity.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSetGameState : MonoBehaviour {

    [SerializeField]
    GAME_STATE setGameState;

    Button button;

    private void Awake() {
        this.button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy() {
        button.onClick.RemoveAllListeners();
    }

    private void OnButtonClick() {
        GameManager.Instance.SetGameState(setGameState);
    }
}