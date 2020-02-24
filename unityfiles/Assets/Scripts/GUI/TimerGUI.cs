using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerGUI : MonoBehaviour {

    [SerializeField] 
    private TMP_Text timerText;
    
    [SerializeField]
    private float levelTime = 1000;

    private string timerID = "1";
    private Timers timer;

    private void Awake() {
        if (timerText == null){
            throw new MissingComponentException("Missing text component");
        }
        this.timer = new Timers();
        timer.Set(timerID, levelTime);
    }

    private void Update() {
        SetTimerText();
    }

    private void SetTimerText() {
        this.timerText.SetText($"{timer.TimeLeft(timerID)}");
    }
}
