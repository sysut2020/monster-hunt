using System;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is responsible for updating the GUI with how many letters
/// that is collected and how many letters there are left to collect.
/// </summary>
public class LettersCollectedGUI : MonoBehaviour {

    /// <summary>
    /// Text for visualizing numbers of letters collected and have left.
    /// </summary>
    [SerializeField]
    TMP_Text letterCounter;

    private int currentLetterCount = 0;

    private static Vector3 resultPosition;

    private static RectTransform myRectTransform;
    
    /// <summary>
    /// How many letters to collect 
    /// </summary>
    private int lettersToCollect = 0;

    private void Awake() {
        if (letterCounter == null){
            throw new MissingComponentException("Missing text component");
        }
        LetterCollectable.OnLetterCollected += OnNewLetter;
        SetLetterText();
    }

    private void SetLetterText() {
        this.letterCounter.SetText($"{currentLetterCount} / {lettersToCollect}");
    }

    private void OnNewLetter(object sender, LetterCollectedArgs letter) {
        this.currentLetterCount++;
        this.SetLetterText();
    }

    /// <summary>
    /// Sets how many letters to collect. Must be a size larger or equal to 0
    /// </summary>
    /// <param name="size">number of letters to collect >= 0</param>
    public void SetLettersToCollectCount(int count) {
        if (count < 0){
            throw new ArgumentOutOfRangeException("Count cant be negative");
        }
        this.lettersToCollect = count;
    }

    private void OnDestroy() {
        LetterCollectable.OnLetterCollected -= OnNewLetter;
    }
}