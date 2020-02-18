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

    private static GameObject letterGUIObject;

    /// <summary>
    /// How many letters to collect 
    /// </summary>
    private int lettersToCollect = 0;

    private void Awake() {
        if (letterCounter == null) throw new MissingComponentException("Missing text component");
        Letter.OnLetterCollected += OnNewLetter;
        SetLetterText();

        letterGUIObject = new GameObject("World letter position");
        TryGetComponent<RectTransform>(out myRectTransform);
    }

    /// <summary>
    /// Used to convert the GUI position of the letter GUI tab at the games camera,
    /// to its position in game.
    /// </summary>
    private void FixedUpdate() {
        Vector2 vectorRectTransformPosition = myRectTransform.transform.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(myRectTransform, vectorRectTransformPosition,
            FindObjectOfType<Camera>(), out resultPosition);
        letterGUIObject.transform.position = resultPosition;
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
        if (count < 0) throw new ArgumentOutOfRangeException("Count cant be negative");
        this.lettersToCollect = count;
    }

    private void OnDestroy() {
        Letter.OnLetterCollected -= OnNewLetter;
    }

    public static Transform TryGetTransform() {
        return letterGUIObject.transform;
    }
}