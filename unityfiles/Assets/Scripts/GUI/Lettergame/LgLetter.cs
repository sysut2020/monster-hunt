using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGameLetter {

    private readonly string letter;
    private int xPos, yPos;
    private bool isOnBoard = false;
    private Action<bool> OnValidLetterInWord { get; set; }
    private float validationStamp;
    private bool isValid;

    public GameBoardTile gbt;

    public LetterGameLetter(int x, int y, string tileLetter) {
        this.XPos = x;
        this.YPos = y;
        this.letter = tileLetter;
    }

    /// <summary>
    /// Registers a callback for when the letter becomes a valid/unvalid
    /// letter in a word.
    /// </summary>
    /// <param name="func">callback function to be called</param>
    public void OnValidLetterInWordCallback(Action<bool> func) {
        OnValidLetterInWord = func;
    }

    /// <summary>
    /// Sets the letter to be valid or invalid: true is a valid letter.
    /// Triggers the registered callback method when it becomes valid.
    /// </summary>
    /// <param name="isValidLetterInWord">true if valid, false if unvalid</param>
    /// <param name="validationStamp">an unique timestamp for when it was validated</param>
    public void SetValidLetter(bool isValidLetterInWord, float validationStamp) {
        try {
            if (this.validationStamp.Equals(validationStamp) && isValid) return;
            this.validationStamp = validationStamp;
            this.isValid = isValidLetterInWord;
            OnValidLetterInWord(isValidLetterInWord);
        } catch (System.NullReferenceException) { }
    }

    public string Letter { get => letter; }
    public bool IsOnBoard { get => isOnBoard; set => isOnBoard = value; }
    public bool isValidLetterInWord { get => isValid; set => isValid = value; }
    public int YPos { get => yPos; set => yPos = value; }
    public int XPos { get => xPos; set => xPos = value; }
}