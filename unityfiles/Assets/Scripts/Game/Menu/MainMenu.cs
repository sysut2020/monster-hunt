﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public void PlayGame() {
        GameManager.Instance.GameStateChange(GAME_STATE.START_GAME);
    }

    public void QuitGame() {
        GameManager.Instance.GameStateChange(GAME_STATE.EXIT);
    }
}
