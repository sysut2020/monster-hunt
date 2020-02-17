using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.Instance.GameStateChange(STATE.TEST_LEVEL);
    }

    public void QuitGame()
    {
        GameManager.Instance.GameStateChange(STATE.EXIT_GAME);
        
    }
}
