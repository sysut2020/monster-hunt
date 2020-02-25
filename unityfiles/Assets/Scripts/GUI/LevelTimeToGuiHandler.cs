using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimeToGuiHandler : MonoBehaviour{
	
	private TimerGUI timerGUI;

	private LevelManager levelManager;

	private void Start() {
		if (TryGetComponent(out TimerGUI timerGuiSript)){
			this.timerGUI = timerGuiSript;
		}else{
			Debug.LogError("Cant find timer GUI");
		}
	}
	
	private void Update() {
		this.timerGUI.SetTime(LevelManager.Instance.GetLevelTimeLeft());
	}


}