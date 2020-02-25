using System;
using System.Collections.Generic;
using UnityEngine;

public class GuiHandler : MonoBehaviour{

	private TimerGUI timerGUI;

	private LevelManager levelManager;

	private void Start() {
		if (TryGetComponent(out TimerGUI timerGuiSript)){
			this.timerGUI = timerGuiSript;
		}else{
			Debug.LogError("Cant find timer gui");
		}
	}

	private void Update() {
		this.timerGUI.SetTime(LevelManager.Instance.GetLevelTimeLeft());
	}


}