using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevel : MonoBehaviour {

    public void PlayLetterGame() {
        GameManager.Instance.GameStateChange(GAME_STATE.LETTER_LEVEL);
    }
}
