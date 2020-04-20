using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Responsible for displaying time on a text element.
/// It displays time in format Minutes:Seconds.
/// </summary>
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
    private int levelTime;
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

    /// <summary>
    /// Sets the time to display. Time in milliseconds.
    /// </summary>
    /// <param name="time">time to display, in milliseconds</param>
    public void SetTime(int time) {
        this.LevelTime = time / 1000;
    }

    private void SetTimerText() {
        // If the level time has changed, update the timer GUI
        if (this.levelTime != lastTime) {
            this.lastTime = this.levelTime;

            TimeSpan timeSpan = TimeSpan.FromSeconds(levelTime);
            // Used to format the time to a readable "human" time
            string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            this.timerText.SetText($"{timeString}");
        }

    }
}