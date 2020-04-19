using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Represents a tile in the letter mini game
/// Should hold a uniq letter
/// Keeps track of how many units of that particular letter the player has left to be placed on the board 
/// </summary>
public class LetterTile : Dragable {
    [SerializeField]
    private TMP_Text LetterCount;

    [SerializeField]
    private TMP_Text DisplayLetter;

    [SerializeField]
    private string letterTileLetter = "#";

    private LetterGameLetter holdingLetter;

    public string LetterTileLetter {
        get => letterTileLetter;
        set {
            this.letterTileLetter = value;
            this.IconLetter = value;
            this.DisplayLetter.text = value;
        }
    }

    /// <summary>
    /// Callback for the letter count change event
    /// </summary>
    /// <param name="_"></param>
    /// <param name="args">event args</param>
    private void CallbackLetterCountChangedEvent (object _, LetterCountChangedEventArgs args) {
        Dictionary<string, int> letterCounts = args.AvailLetters;

        if (letterCounts.ContainsKey (LetterTileLetter)) {
            LetterCount.text = letterCounts[LetterTileLetter].ToString ();
        }
    }

    /// <summary>
    /// Tells whether or not the Ui element should start the drag operation 
    /// </summary>
    /// <returns>true if the drag operation can start false if not</returns>   
    override protected bool CanStartDrag () {
        holdingLetter = LetterLevelController.Instance.TryGetLetter (IconLetter);
        return holdingLetter != null;
    }

    /// <summary>
    /// What the UI element should do when the drag operation is complete
    /// </summary>
    override protected void OnDragCompletion (PointerEventData eventData) {
        GameObject hit = eventData.pointerCurrentRaycast.gameObject;
        if (holdingLetter == null || hit == null) {
            return;
        }

        if (hit.TryGetComponent<GameBoardTile> (out GameBoardTile tile)) {
            tile.SetTile (holdingLetter);
            holdingLetter = null;
        }
    }

    void Awake () {
        LetterLevelController.LetterCountChangedEvent += CallbackLetterCountChangedEvent;
    }

    private void OnDestroy () {
        LetterLevelController.LetterCountChangedEvent -= CallbackLetterCountChangedEvent;
    }
}