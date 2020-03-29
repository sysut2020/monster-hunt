using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button handler for changing level state.
/// It requires to be on an game object with Button script applied
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonChangeLevelState : ChangeEventButton {
    [SerializeField]
    private PAUSE_MENU_EVENTS eventToFire;
    public static event EventHandler<ButtonClickEventArgs> buttonEventHandler;
    /// <summary>
    /// 
    /// 
    /// 
    /// SUPER TODO: IMPLEMENT THIS
    ///             What is in here is the Menu button stuff
    /// 
    /// 
    /// 
    /// </summary>


    protected override Enum EventToFire => eventToFire;
    protected override EventHandler<ButtonClickEventArgs> ButtonEventHandler => buttonEventHandler;
}
