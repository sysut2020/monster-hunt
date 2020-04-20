using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TileChangedEventArgs : EventArgs {
    private String Letter { get; set; }
}

/// <summary>
/// Used to handle the letter game board tile
/// </summary>
public class LetterGameBoardTile : DragableLetter {

    [SerializeField]
    private TMP_Text TileText;

    [SerializeField]
    private int xPos, yPos;

    [SerializeField]
    private Image tileImage;

    [SerializeField]
    private Image verticalIndicator;

    [SerializeField]
    private Image horizontalIndicator;

    private LetterGameLetter holdingLetter = null;

    public int XPos { get => xPos; set => xPos = value; }
    public int YPos { get => yPos; set => yPos = value; }

    /// <summary>
    /// Set the letter to be displayed in the tile, and 
    /// register callback for when/if the tiles becomes 
    /// valid in a word.
    /// </summary>
    /// <param name="letter">the letter to display</param>
    public void SetTile(LetterGameLetter letter) {
        letter.OnValidLetterInWordCallback(SetTileValidColor);
        holdingLetter = letter;
        LetterLevelController.Instance.UpdateLetterPos(XPos, YPos, letter);
        this.updateDisplayedLetter();
    }

    /// <summary>
    /// resets the displayed char on the tile
    /// </summary>
    public void ResetTile() {
        SetTileValidColor(false, Direction.CENTER);
        holdingLetter = null;
        this.updateDisplayedLetter();
    }
    /// <summary>
    /// Sets the color on the tile if it is valid, else 
    /// remove color
    /// </summary>
    private void SetTileValidColor(bool isValid, Direction direction) {
        if (isValid) {
            this.tileImage.color = Color.green;
            switch (direction) {
                case Direction.RIGHT:
                    this.verticalIndicator.enabled = true;
                    break;
                case Direction.DOWN:
                    this.horizontalIndicator.enabled = true;
                    break;
                default:
                    break;
            }
        } else {
            this.tileImage.color = Color.white;
            this.horizontalIndicator.enabled = false;
            this.verticalIndicator.enabled = false;
        }

    }

    /// <summary>
    /// updates the char displayed 
    /// </summary>
    private void updateDisplayedLetter() {
        if (holdingLetter == null) {
            TileText.text = "";
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
            TileText.text = "";
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
            if (hit.TryGetComponent<LetterGameBoardTile>(out LetterGameBoardTile tile)) {
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
                LetterLevelController.Instance.UpdateLetterPos(-1, -1, holdingLetter);
                ResetTile();

            }
        } else {
            // hit nothing
            LetterLevelController.Instance.UpdateLetterPos(-1, -1, holdingLetter);
            ResetTile();
        }

        updateDisplayedLetter();
    }

    private void Awake() {
        TileText = this.GetComponentInChildren<TMP_Text>();
    }
}