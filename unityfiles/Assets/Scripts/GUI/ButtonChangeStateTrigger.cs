using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickEventArgs: EventArgs{
    public Enum ButtonEvent {get;set;}
}

/// <summary>
/// the abstract class containing for a button that fires an event
/// The implementation of this abstract class has to define the
/// get field for the Event enumerator and the event handler to be invoked
/// </summary>
public abstract class ChangeEventButton : MonoBehaviour {
    /// <summary>
    /// The button that triggers the click event
    /// </summary>
    private Button triggerButton;

    /// <summary>
    /// The enumerator containing the button event
    /// </summary>
    abstract protected Enum EventToFire{get;}

    /// <summary>
    /// The event handler to Invoke
    /// </summary>
    abstract protected EventHandler<ButtonClickEventArgs> ButtonEventHandler{get;}
    

    private void Awake() {
        this.triggerButton = GetComponent<Button>();
        this.triggerButton.onClick.AddListener(ChangeState);
    }

    private void OnDestroy() {
        this.triggerButton.onClick.RemoveListener(ChangeState);
    }

    private void ChangeState(){
        ButtonClickEventArgs args = new ButtonClickEventArgs();
        args.ButtonEvent = EventToFire;
        ButtonEventHandler?.Invoke (this, args);
    }
}