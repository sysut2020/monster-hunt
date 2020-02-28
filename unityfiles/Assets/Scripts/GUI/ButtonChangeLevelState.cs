using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button handler for changing level state.
/// It requires to be on an game object with Button script applied
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonChangeLevelState : ButtonChangeStateTrigger
{
    [SerializeField]
    private LEVEL_STATE levelState;

    protected override void ChangeState(){
        LevelManager.Instance.ChangeLevelState(this.levelState);
    }
}
