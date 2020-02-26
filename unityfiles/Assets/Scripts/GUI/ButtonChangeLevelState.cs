﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button handler for changing level state.
/// It requires to be on an game object with Button script applied
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonChangeLevelState : MonoBehaviour
{

    [SerializeField]
    private LEVEL_STATE levelState;
    
    /// <summary>
    /// The button that triggers the click event
    /// </summary>
    private Button triggerButton;

    private void Awake() {
        this.triggerButton = GetComponent<Button>();
        this.triggerButton.onClick.AddListener(ChangeState);
    }

    private void OnDestroy() {
        this.triggerButton.onClick.RemoveListener(ChangeState);
    }

    private void ChangeState(){
        LevelManager.Instance.ChangeLevelState(this.levelState);
    }
}
