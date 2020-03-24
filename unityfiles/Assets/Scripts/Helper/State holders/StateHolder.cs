using System;





/// <summary>
/// 
/// </summary>
public enum PLAYER_ANIMATION {

    /// <summary>
    /// 
    /// </summary>
    HOLD_SNIPER = 1,

    /// <summary>
    /// 
    /// </summary>
    HOLD_RAYGUN = 2
}

public enum PAUSE_MENU_STATE {
    /// <summary>
    /// Waiting for player confirmation state
    /// </summary>
    CONFIRMATION,

    /// <summary>
    /// The base state of the pause menu
    /// </summary>
    BASE,

    /// <summary>
    /// Quiting state of pause menu
    /// </summary>
    QUIT
}