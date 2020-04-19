using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGameLetter {

	private readonly string letter;
	private readonly int scoreValue;
	private int xPos, yPos;
	private bool isOnBoard = false;
	private Action<bool, Direction> OnValidLetterInWord { get; set; }
	private bool isValid;
	public LetterGameLetter(int x, int y, string tileLetter, int scoreValue) {
		this.XPos = x;
		this.YPos = y;
		this.letter = tileLetter;
		this.scoreValue = scoreValue;
	}

	/// <summary>
	/// Registers a callback for when the letter becomes a valid/unvalid
	/// letter in a word.
	/// </summary>
	/// <param name="func">callback function to be called</param>
	public void OnValidLetterInWordCallback(Action<bool, Direction> func) {
		OnValidLetterInWord = func;
	}

	/// <summary>
	/// Sets the letter to be valid or invalid: true is a valid letter.
	/// Triggers the registered callback method when it becomes valid.
	/// </summary>
	/// <param name="isValidLetterInWord">true if valid, false if unvalid</param>
	public void SetValidLetter(bool isValidLetterInWord, Direction direction) {
		try {
			this.isValid = isValidLetterInWord;
			OnValidLetterInWord(isValidLetterInWord, direction);
		} catch (NullReferenceException) { }
	}

	public string Letter { get => letter; }
	public bool IsOnBoard { get => isOnBoard; set => isOnBoard = value; }
	public bool isValidLetterInWord { get => isValid; set => isValid = value; }
	public int YPos { get => yPos; set => yPos = value; }
	public int XPos { get => xPos; set => xPos = value; }

	public int ScoreValue => scoreValue;
}