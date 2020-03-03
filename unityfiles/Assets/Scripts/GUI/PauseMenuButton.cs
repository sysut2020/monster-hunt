using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuButton : ChangeEventButton {
    [SerializeField]
    private PAUSE_MENU_STATE eventToFire;

    public static event EventHandler<ButtonClickEventArgs> buttonEventHandler;


    protected override Enum EventToFire => eventToFire;
    protected override EventHandler<ButtonClickEventArgs> ButtonEventHandler => buttonEventHandler;
}