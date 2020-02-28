using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonChangeStateTrigger : MonoBehaviour {
    /// <summary>
    /// The button that triggers the click event
    /// </summary>
    protected Button triggerButton;
    
    private void Awake() {
        this.triggerButton = GetComponent<Button>();
        this.triggerButton.onClick.AddListener(ChangeState);
    }

    private void OnDestroy() {
        this.triggerButton.onClick.RemoveListener(ChangeState);
    }

    protected abstract void ChangeState();
}