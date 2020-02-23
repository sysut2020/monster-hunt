using System;


/// <summary>
/// The constants for the different game states
/// </summary>
public enum GAME_STATE {
    /// <summary>
    /// The Game should exit
    /// </summary>
	EXIT,

    /// <summary>
    /// Go to main menu
    /// </summary>
	MAIN_MENU,

    /// <summary>
    /// Go to the test level
    /// </summary>
	TEST_LEVEL,
}

/// <summary>
/// The constants for the different level states
/// </summary>
public enum LEVEL_STATE {

    /// <summary>
    /// Exit the current level
    /// </summary>
    EXIT,
    
    /// <summary>
    /// Go to game over screen
    /// </summary>
	GAME_OVER,

    /// <summary>
    /// Start the hunting mode in game
    /// </summary>
    HUNTING,
    /// <summary>
    /// Reloads the current scene
    /// </summary>
    RELOAD,
    
    GAME_WON
}

