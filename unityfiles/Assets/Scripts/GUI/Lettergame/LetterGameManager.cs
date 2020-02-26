using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -- properties -- //
// -- events -- // 
// -- public -- //
// -- private -- // 
// -- unity -- //

public class LetterGameStartEventArgs: EventArgs{
    public Dictionary<string, int> CurrentLetters {get; set;}
}

public class BoardChangedEventArgs: EventArgs{
    private LgLetter[,] TileMap { get; set; }
}

public class LetterCountCangedEventArgs: EventArgs{
    public Dictionary<string, int> AvailLetters {get; set;}
}


/// <summary>
/// NOTES: this may very well be a stupid and unreliable way of doing this a if tuff brakes weirdly (mismatch in the array an the real board)
///     just make an event that tels every tile how the board should look
/// 
/// This class shod also kinda be two classes but that create a ridiculous high Coupling
/// 
/// 
/// stuff to implement find a smart way to notefy all letters shy shold be completd
/// </summary>
public class LetterGameManager : Singleton<LetterGameManager>
{
    // maby move this to a global constant
    private readonly string[] letters = {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

    [Tooltip("")]
    [SerializeField]
    private int bSizeX;

    [Tooltip("")]
    [SerializeField]
    private int bSizeY;



    [SerializeField]
    GameObject playingBoard;

    [SerializeField]
    GameObject boardTile;


    [SerializeField]
    GameObject letterBoard;

    [SerializeField]
    GameObject letterTile;

    

    [SerializeField]
    GameObject icon;



    private Dictionary<String, List<LgLetter>> playerLetters; 
    private LgLetter[,] tileMap;



    
    // -- properties -- //
    public Dictionary<string, int> CurrentAvailableLetterCount{
        get{
            Dictionary<string, int> ret = new Dictionary<string, int>();
            foreach (string key in playerLetters.Keys){
                ret.Add(key, 0);
                foreach (LgLetter l in playerLetters[key]){
                    if (!l.IsOnBoard) ret[key] += 1;
                }
            }
            return ret;
        }
    }
    // -- events -- // 
    public static event EventHandler<BoardChangedEventArgs> BoardChangedEvent;
    public static event EventHandler<LetterCountCangedEventArgs> LetterCountCangedEvent;


    private void CallbackLetterGameStartEvent(object _, LetterGameStartEventArgs args){

    }

    // -- public -- //
    public LgLetter TryGetLetter(string letter){
        LgLetter ret = null;
        foreach (LgLetter l in playerLetters[letter]){
            if (!l.IsOnBoard){
                ret = l;
                break;
            }
        }
        this.refreshLetterNumberDisplay();
        return ret;
    }

    public void UpdateLetterPos(int newX, int newY, LgLetter letter){
        if (BoardIsTileValid(newX, newY)){
            // check for new words from the new point
            this.BoardSetTile(newX,newY,letter);
            this.ChekIfWordFromPos(newX,newY);
        } else {
            // a letter til has been removed from the screen remove it
            BoardTryRemoveLetter(letter);
        }

        // check if the was any words formed from moving this one

        // TODO: Currently cheking xx AND y for all laboring nodes when only x for t/b and y for l/r should be cheked
        newX = letter.XPos;
        newY = letter.YPos;
        print(BoardIsTileValid(newX+1, newY));
        
        if (BoardIsTileValid(newX+1, newY)) ChekIfWordFromPos(newX+1, newY);
        if (BoardIsTileValid(newX-1, newY)) ChekIfWordFromPos(newX-1, newY);
        if (BoardIsTileValid(newX, newY+1)) ChekIfWordFromPos(newX, newY+1);
        if (BoardIsTileValid(newX, newY-1)) ChekIfWordFromPos(newX, newY-1);
        
        this.refreshLetterNumberDisplay();
    }
    
    // -- private -- //

    private void ChekIfWordFromPos(int x, int y){
        
        if (!BoardIsTileValid(x,y)){return;}
        if (BoardTryGetTile(x,y) == null){return;}
        var xDir = WUArrays.GetConnected(tileMap,x,y,0).Select(tile => tile.Letter).ToArray();
        var yDir = WUArrays.GetConnected(tileMap,x,y,1).Select(tile => tile.Letter).ToArray();

        string wordXN = string.Join("",xDir.ToArray());
        string wordXR = string.Join("",xDir.Reverse().ToArray());

        string wordYN = string.Join("",yDir.ToArray());
        string wordYR = string.Join("",yDir.Reverse().ToArray());

        //print($"X dir {wordXN}");
        //print($"Y dir {wordYN}");
        if(
            WordChecker.isWordValid(wordXN)||
            WordChecker.isWordValid(wordXR)||
            WordChecker.isWordValid(wordYN)||
            WordChecker.isWordValid(wordYR)
        ){
            print("🎂🎉🎉🎉🎂🎂 WORD FOUND 🎂🎂🎂🎉🎂🎉🎉");
        }
    }

    private void refreshLetterNumberDisplay(){
        LetterCountCangedEventArgs args = new LetterCountCangedEventArgs();
        args.AvailLetters = this.CurrentAvailableLetterCount;
        LetterCountCangedEvent?.Invoke(this,args);
    }


    private void FillPlayerLetters(LetterGameStartEventArgs args){
        Dictionary<string, int> playerDataDict = new Dictionary<string, int> {{"A",5},{"B",7},{"C",4}};// args.CurrentLetters; 
        foreach (string key in playerDataDict.Keys){
           if(playerLetters.Keys.Contains(key)){
               for (int i = 0; i < playerDataDict[key]; i++){
                    LgLetter newLetter = new LgLetter(-1,-1, key); 
                    playerLetters[key].Add(newLetter);
               }
               
           }
        }
    }

    private void MakePlayerLetter(){
        foreach (String letter in letters)
        {
           this.playerLetters.Add(letter, new List<LgLetter>());
        }
    }


    

    // -- tile map stuff -- //

    private bool BoardTryRemoveLetter(LgLetter letter){
        bool suc = false;
        if (letter.IsOnBoard){ // fixe seinere WUArrays.MultiDimFind(tileMap, letter)
            if (letter != tileMap[letter.XPos,letter.YPos]){
                throw new Exception("TILE POSSISION MISMATCH");
            }
            
            tileMap[letter.XPos,letter.YPos] = null;
            letter.IsOnBoard = false;
            letter.XPos = -1;
            letter.YPos = -1;

            this.refreshLetterNumberDisplay();
            suc = true;
        }
        return suc;
    }
    
    private void BoardSetTile(int x, int y, LgLetter tile){
        LgLetter oldTile = this.BoardTryGetTile(x,y);
        if (oldTile != null){
            // if there is a tile at the possision remove it
            this.BoardTryRemoveLetter(oldTile);
        } 

        
        tile.XPos = x;
        tile.YPos = y;
        tile.IsOnBoard = true;
        tileMap[x,y] = tile;
        print($"New tile set at x{x}-y{y}");        
    }

    private LgLetter BoardTryGetTile(int x, int y){
        LgLetter ret = null;

        if (BoardIsTileValid(x,y)){
            if(tileMap[x,y] != null){
                ret = tileMap[x,y];
            }
        }
        
        return ret;
    }

    private bool BoardIsTileValid(int x, int y){
        bool ret = true;
        if (
            x > tileMap.GetUpperBound(0)||
            x < tileMap.GetLowerBound(0)||
            y > tileMap.GetUpperBound(1)||
            y < tileMap.GetLowerBound(1)
            ){ret = false;}
        return ret;
    }


   


    // Start is called before the first frame update
    void Start(){
        this.tileMap = new LgLetter[this.bSizeX,this.bSizeY];
        this.playerLetters = new Dictionary<String, List<LgLetter>>();
        MakePlayerLetter();
        FillPlayerLetters(null);
        

        for (int i = 0; i < bSizeX*bSizeY; i++){
            GameObject n = Instantiate(this.boardTile);
            GameBoardTile tile = n.GetComponent<GameBoardTile>();
            tile.XPos = (int) i % bSizeX;
            tile.YPos = (int) Mathf.Floor(i/bSizeX);
            n.transform.SetParent(playingBoard.transform);

        }

        foreach (string letter in letters){
            GameObject n = Instantiate(this.letterTile);
            LetterTile tile = n.GetComponent<LetterTile>();
            tile.LetterTileLetter = letter;
            n.transform.SetParent(letterBoard.transform);
        }

        refreshLetterNumberDisplay();
        
    }

}
