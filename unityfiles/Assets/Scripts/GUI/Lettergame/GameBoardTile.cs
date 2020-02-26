using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TileChangedEventArgs: EventArgs{
    private String Letter { get; set; }
}

public class GameBoardTile : Dragable
{

    [SerializeField]
    private Text TileText;



    [SerializeField]
    private int xPos, yPos;

    private LgLetter holdingLetter;

    public int XPos { get => xPos; set => xPos = value; }
    public int YPos { get => yPos; set => yPos = value; }

    // -- properties -- //
    // -- events -- // 
    public static event EventHandler<TileChangedEventArgs> TileChangedEventArgs;
    public void CallbackBoardChangedEvent(object _, BoardChangedEventArgs __){

    }

    // -- public -- //
    public void SetTile(LgLetter letter){
        LetterGameManager.Instance.UpdateLetterPos(XPos,YPos,letter);
        holdingLetter = letter;
        this.updateDisplayedLetter();
    }

    
    // -- private -- // 

    public void ResetTile(){
        holdingLetter=null;
        this.updateDisplayedLetter();
    }

    private void updateDisplayedLetter(){
        if (holdingLetter == null){
            TileText.text ="?";
        }else{
            TileText.text = this.holdingLetter.Letter;
        }
        
    }

    override protected bool CanStartDrag(){
        if (holdingLetter != null){
            this.IconLetter = this.holdingLetter.Letter;
            TileText.text ="?";
            return true;
        }
        return false;

    }

    override protected void OnDragCompletion(PointerEventData eventData){
        GameObject hit = eventData.pointerCurrentRaycast.gameObject;
        if (hit != null){
            if( hit.TryGetComponent<GameBoardTile>(out GameBoardTile tile)){
                if (tile != this){
                    tile.SetTile(holdingLetter);
                    ResetTile();
                }
            }
        } else {
            LetterGameManager.Instance.UpdateLetterPos(-1,-1,holdingLetter);
            ResetTile();
        }

        updateDisplayedLetter();
    }
    // -- unity -- //



    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void setText(string s){
        TileText.text = s;
    }

    private void Start() {
        TileText = this.GetComponentInChildren<Text>();
    }

    private void OnDestroy() {
        //GameBoard.BoardChangedEvent += CallbackBoardChangedEvent;
    }
}
