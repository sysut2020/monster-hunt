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
    private float levelTime = 10;
    public float LevelTime {
        get => levelTime;
        set => levelTime = value;
    }

    private void Awake() {
        if (timerText == null){
            throw new MissingComponentException("Missing text component");
        }
    }

    private void Update() {
        SetTimerText();
    }
    
    private void SetTimerText() {
        levelTime -= Time.deltaTime;
        if (levelTime < 0) {
            levelTime = 0;
        }
        
        //Rounds the level time to an even number
        Mathf.RoundToInt(levelTime).ToString();
        
        TimeSpan timeSpan = TimeSpan.FromSeconds(levelTime);
        // Used to format the time to a readable "human" time
        string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        
        this.timerText.SetText($"{timeString}");
    }
}
