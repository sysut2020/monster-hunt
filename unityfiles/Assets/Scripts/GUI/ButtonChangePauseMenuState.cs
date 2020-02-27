using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangePauseMenuState : ButtonTrigger {
    [SerializeField]
    private PAUSE_MENU_STATE pauseState;

    private void Awake() {
        this.triggerButton = GetComponent<Button>();
        this.triggerButton.onClick.AddListener(ChangeState);
    }

    private void OnDestroy() {
        this.triggerButton.onClick.RemoveListener(ChangeState);
    }

    private void ChangeState() {
        PauseMenu.Instance.ChangePauseMenuState(this.pauseState);
    }
}