using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Changes to next level
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonChangeToNextLevel : ChangeEventButton {
    private readonly GAME_STATE eventToFire = GAME_STATE.NEXT_LEVEL;

    public static event EventHandler<ButtonClickEventArgs> buttonEventHandler;
    protected override Enum EventToFire => eventToFire;
    protected override EventHandler<ButtonClickEventArgs> ButtonEventHandler => buttonEventHandler;
}