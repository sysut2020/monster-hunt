using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LetterTile : Dragable
{

    

    [SerializeField]
    private Text LetterCount;

    [SerializeField]
    private Text DisplayLetter;

    [SerializeField]
    private string letterTileLetter = "#";

    private LgLetter holdingLetter;

    public string LetterTileLetter { get => letterTileLetter; set => letterTileLetter = value; }



    // -- properties -- //


    // -- events -- // 

    private void CallbackLetterCountChangedEvent(object _, LetterCountCangedEventArgs args){
        Dictionary<string, int> lettercounts = args.AvailLetters;

        if (lettercounts.ContainsKey(LetterTileLetter)){
            LetterCount.text =  lettercounts[LetterTileLetter].ToString();
        }
    }
    // -- public -- //
    // -- private -- // 
    override protected bool CanStartDrag(){
        holdingLetter = null;
        holdingLetter = LetterGameManager.Instance.TryGetLetter(IconLetter);
        return holdingLetter != null;

    }

    override protected void OnDragCompletion(PointerEventData eventData){
        if (holdingLetter != null){
            GameObject hit = eventData.pointerCurrentRaycast.gameObject;
            if (hit != null){
                if( hit.TryGetComponent<GameBoardTile>(out GameBoardTile tile)){
                    tile.SetTile(holdingLetter);
                    holdingLetter = null;
                }
            }
        }
    }
    // -- unity -- //

    
    

  


    // Start is called before the first frame update
    void Start(){
        this.IconLetter = this.LetterTileLetter;
        this.DisplayLetter.text = this.letterTileLetter;
        LetterGameManager.LetterCountCangedEvent += CallbackLetterCountChangedEvent;
        
    }

    private void OnDestroy() {
        LetterGameManager.LetterCountCangedEvent -= CallbackLetterCountChangedEvent;
    }



}
