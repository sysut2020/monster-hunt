using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserNameGUIController : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI input;
    private GameDataManager dataManager;
    void Awake() {
        if (input == null) {
            throw new MissingComponentException("Missing TextMeshPro name component");
        }
    }

    void Start() {
        dataManager = GameManager.Instance.GameDataManager;
    }

    public void SaveHighScoreName() {
        dataManager.HighScoreName = input.text;
        dataManager.SaveData();
    }
}