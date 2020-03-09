using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TileChangedEventArgs : EventArgs {
    private String Letter { get; set; }
}

public class GameBoardTile : Dragable {

    [SerializeField]
    private TMP_Text TileText;

    [SerializeField]
    private int xPos, yPos;

    private LetterGameLetter holdingLetter = null;

    public int XPos { get => xPos; set => xPos = value; }
    public int YPos { get => yPos; set => yPos = value; }

    // -- properties -- //
    // -- events -- // 

    // -- public -- //

    /// <summary>
    /// set the letter to be displayed int the tile
    /// </summary>
    /// <param name="letter">the letter to display</param>
    public void SetTile(LetterGameLetter letter) {
        LetterGameManager.Instance.UpdateLetterPos(XPos, YPos, letter);
        holdingLetter = letter;
        this.updateDisplayedLetter();
    }

    // -- private -- // 

    /// <summary>
    /// resets the displayed char on the tile
    /// </summary>
    public void ResetTile() {
        holdingLetter = null;
        this.updateDisplayedLetter();
    }

    /// <summary>
    /// updates the char displayed 
    /// </summary>
    private void updateDisplayedLetter() {
        if (holdingLetter == null) {
            TileText.text = "?";
        } else {
            TileText.text = this.holdingLetter.Letter;
        }

    }

    /// <summary>
    /// Tells whether or not the Ui element should start the drag operation 
    /// </summary>
    /// <returns>true if the drag operation can start false if not</returns>   
    override protected bool CanStartDrag() {
        if (holdingLetter != null) {
            this.IconLetter = this.holdingLetter.Letter;
            TileText.text = "?";
            return true;
        }
        return false;

    }

    /// <summary>
    /// What the Ui element should do when the drag operation is complete
    /// </summary>
    override protected void OnDragCompletion(PointerEventData eventData) {
        GameObject hit = eventData.pointerCurrentRaycast.gameObject;
        if (hit != null) {
            // hit something
            if (hit.TryGetComponent<GameBoardTile>(out GameBoardTile tile)) {
                // hit something with game tile
                if (tile != this) {
                    // not self
                    if (holdingLetter != null) {
                        // if holding a letter
                        tile.SetTile(holdingLetter);
                        ResetTile();
                    }

                }
            } else {
                // hit some GO without game tile
                LetterGameManager.Instance.UpdateLetterPos(-1, -1, holdingLetter);
                ResetTile();

            }
        } else {
            // hit nothing
            LetterGameManager.Instance.UpdateLetterPos(-1, -1, holdingLetter);
            ResetTile();
        }

        updateDisplayedLetter();
    }
    // -- unity -- //

    private void Awake() {
        TileText = this.GetComponentInChildren<TMP_Text>();
    }
}