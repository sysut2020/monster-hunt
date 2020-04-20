using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for adding a new score entry to persistant storage. This is done
/// when the input field contains char length > 0 and the user clicks the registered
/// button which fires the click event.
/// </summary>
public class AddScoreEntryController : MonoBehaviour {

    [SerializeField]
    private TMP_InputField entryNameField;

    [SerializeField]
    private Button addEntryButton;

    private GameDataManager dataManager;

    private void Awake() {
        if (entryNameField == null) {
            throw new MissingComponentException("Missing TextMeshPro name component");
        }
        if (addEntryButton == null) {
            throw new MissingComponentException("Missing add entry button component");
        }
        entryNameField.onValueChanged.AddListener(OnInputfieldChanged);
    }

    private void Start() {
        addEntryButton.interactable = false;
        dataManager = GameManager.Instance.GameDataManager;
    }

    private void OnDestroy() {
        entryNameField.onValueChanged.RemoveListener(OnInputfieldChanged);
    }

    private void OnInputfieldChanged(string newValue) {
        addEntryButton.interactable = newValue.Length > 0;
    }

    /// <summary>
    /// Adds a new entry to the scoreboard, with the text from input field
    /// registered to the object.
    /// </summary>
    public void AddEntry() {
        dataManager.AddNewHighScoreEntry(entryNameField.text);
    }
}