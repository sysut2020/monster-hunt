using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangePauseMenuState : ButtonChangeStateTrigger {
    [SerializeField]
    private PAUSE_MENU_STATE pauseState;
    
    protected override void ChangeState() {
        PauseMenu.Instance.ChangePauseMenuState(this.pauseState);
    }
}