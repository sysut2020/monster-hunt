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

    // -- properties -- //
    public string LetterTileLetter { get => letterTileLetter; set => letterTileLetter = value; }



    


    // -- events -- // 


    /// <summary>
    /// Calback for the letter count change event
    /// </summary>
    /// <param name="args">event args</param>
    private void CallbackLetterCountChangedEvent(object _, LetterCountCangedEventArgs args){
        Dictionary<string, int> lettercounts = args.AvailLetters;

        if (lettercounts.ContainsKey(LetterTileLetter)){
            LetterCount.text =  lettercounts[LetterTileLetter].ToString();
        }
    }


    // -- private -- // 
    
    /// <summary>
    /// Tels whether or not the Ui element should start the drag operation 
    /// </summary>
    /// <returns>true if the drag operation can start false if not</returns>   
    override protected bool CanStartDrag(){
        holdingLetter = null;
        holdingLetter = LetterGameManager.Instance.TryGetLetter(IconLetter);
        return holdingLetter != null;

    }

    /// <summary>
    /// What the Ui element should do when the drag operation is complete
    /// </summary>
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
