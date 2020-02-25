using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerGUI : MonoBehaviour {

    /// <summary>
    /// The text we want displayed in our GUI
    /// </summary>
    [SerializeField]
    private TMP_Text timerText;

    /// <summary>
    /// Time time we want to count down on our timer
    /// </summary>
    [SerializeField]
    [Tooltip("In seconds")]
    private int levelTime = 10;
    public int LevelTime {
        get => levelTime;
        set => levelTime = value;
    }

    private int lastTime = 0;

    private void Awake() {
        if (timerText == null) {
            throw new MissingComponentException("Missing text component");
        }
    }

    private void Update() {
        SetTimerText();
    }

    public void SetTime(int time) {
        this.LevelTime = time;
    }

    private void SetTimerText() {
        // levelTime -= Time.deltaTime;
        // if (levelTime < 0) {
        //     levelTime = 0;
        // }
        if (this.levelTime != lastTime) {
            this.lastTime = this.levelTime;
            //Rounds the level time to an even number
            Mathf.RoundToInt(levelTime).ToString();

            TimeSpan timeSpan = TimeSpan.FromSeconds(levelTime);
            // Used to format the time to a readable "human" time
            string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            this.timerText.SetText($"{timeString}");
        }

    }
}