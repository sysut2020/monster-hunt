using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LetterTile : Dragable {

    [SerializeField]
    private TMP_Text LetterCount;

    [SerializeField]
    private TMP_Text DisplayLetter;

    [SerializeField]
    private string letterTileLetter = "#";

    private LetterGameLetter holdingLetter;

    // -- properties -- //
    public string LetterTileLetter {
        get {
            return letterTileLetter;
        }
        set {
            this.letterTileLetter = value;
            this.IconLetter = value;
            this.DisplayLetter.text = value;
        }
    }

    // -- events -- // 

    /// <summary>
    /// Calback for the letter count change event
    /// </summary>
    /// <param name="args">event args</param>
    private void CallbackLetterCountChangedEvent(object _, LetterCountCangedEventArgs args) {
        Dictionary<string, int> lettercounts = args.AvailLetters;

        if (lettercounts.ContainsKey(LetterTileLetter)) {
            LetterCount.text = lettercounts[LetterTileLetter].ToString();
        }
    }

    // -- private -- // 

    /// <summary>
    /// Tels whether or not the Ui element should start the drag operation 
    /// </summary>
    /// <returns>true if the drag operation can start false if not</returns>   
    override protected bool CanStartDrag() {
        holdingLetter = LetterGameManager.Instance.TryGetLetter(IconLetter);
        return holdingLetter != null;

    }

    /// <summary>
    /// What the Ui element should do when the drag operation is complete
    /// </summary>
    override protected void OnDragCompletion(PointerEventData eventData) {

        GameObject hit = eventData.pointerCurrentRaycast.gameObject;
        if (holdingLetter == null || hit == null) return;

        var gameBoardTile = hit.transform.GetComponentInParent<GameBoardTile>();
        if (gameBoardTile != null) { gameBoardTile.SetTile(holdingLetter); }
        holdingLetter = null;

    }
    // -- unity -- //

    // Start is called before the first frame update
    void Awake() {
        LetterGameManager.LetterCountCangedEvent += CallbackLetterCountChangedEvent;

    }

    private void OnDestroy() {
        LetterGameManager.LetterCountCangedEvent -= CallbackLetterCountChangedEvent;
    }

}