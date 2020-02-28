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
    private LetterGameLetter[,] TileMap { get; set; }
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

    /// <summary>
    /// The x size of the board
    /// </summary>
    [Tooltip("")]
    [SerializeField]
    private int bSizeX;

    /// <summary>
    /// The y size of the board
    /// </summary>
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

    

    private Dictionary<String, List<LetterGameLetter>> playerLetters; 
    private LetterGameLetter[,] tileMap;



    
    // -- properties -- //
    public Dictionary<string, int> CurrentAvailableLetterCount{
        get{
            Dictionary<string, int> ret = new Dictionary<string, int>();
            foreach (string key in playerLetters.Keys){
                ret.Add(key, 0);
                foreach (LetterGameLetter l in playerLetters[key]){
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

    /// <summary>
    /// try's to get a specific letter from the game manager
    /// If a letter is available (not on the board) the letters object is returned
    /// if not null
    /// </summary>
    /// <param name="letter">the letter to check if is available</param>
    /// <returns>the letters objet is it is available else null</returns>
    public LetterGameLetter TryGetLetter(string letter){
        LetterGameLetter ret = null;
        foreach (LetterGameLetter l in playerLetters[letter]){
            if (!l.IsOnBoard){
                ret = l;
                break;
            }
        }
        this.refreshLetterNumberDisplay();
        return ret;
    }

    /// <summary>
    /// Updates the provided letters position to the provided coordinates
    /// </summary>
    /// <param name="newX">the new x pos for the letter</param>
    /// <param name="newY">the new x pos for the letter</param>
    /// <param name="letter">the letter object to update the cords of</param>
    public void UpdateLetterPos(int newX, int newY, LetterGameLetter letter){
        int prevX = letter.XPos;
        int prevY = letter.YPos;
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
        if (BoardIsTileValid(prevX+1, prevY)) ChekIfWordFromPos(prevX+1, prevY);
        if (BoardIsTileValid(prevX-1, prevY)) ChekIfWordFromPos(prevX-1, prevY);
        if (BoardIsTileValid(prevX, prevY+1)) ChekIfWordFromPos(prevX, prevY+1);
        if (BoardIsTileValid(prevX, prevY-1)) ChekIfWordFromPos(prevX, prevY-1);
        
        this.refreshLetterNumberDisplay();
    }
    
    // -- private -- //

    /// <summary>
    /// Makes and places the board tiles
    /// </summary>
    private void MakeBoardTiles(){
        for (int i = 0; i < bSizeX*bSizeY; i++){
            GameObject n = Instantiate(this.boardTile);
            GameBoardTile tile = n.GetComponent<GameBoardTile>();
            tile.XPos = (int) i % bSizeX;
            tile.YPos = (int) Mathf.Floor(i/bSizeX);
            n.transform.SetParent(playingBoard.transform);
        }
    }

    /// <summary>
    /// Makes and places the letter tiles
    /// </summary>
    private void MakeLetterTile(){
        foreach (string letter in letters){
            GameObject n = Instantiate(this.letterTile);
            LetterTile tile = n.GetComponent<LetterTile>();
            tile.LetterTileLetter = letter;
            n.transform.SetParent(letterBoard.transform);
        }
    }

    /// <summary>
    /// Checks if there are any words formed from drawing a vertical or horizontal line 
    /// through the adjacent letters 
    /// </summary>
    /// <param name="x">the x pos of the cord to check from</param>
    /// <param name="y">the x pos of the cord to check from</param>
    private void ChekIfWordFromPos(int x, int y){
        
        if (!BoardIsTileValid(x,y)){return;}
        if (BoardTryGetTile(x,y) == null){return;}
        var xDir = WUArrays.GetConnected(tileMap,x,y,0).Select(tile => tile.Letter).ToArray();
        var yDir = WUArrays.GetConnected(tileMap,x,y,1).Select(tile => tile.Letter).ToArray();

        string wordXN = string.Join("",xDir.ToArray());
        string wordXR = string.Join("",xDir.Reverse().ToArray());

        string wordYN = string.Join("",yDir.ToArray());
        string wordYR = string.Join("",yDir.Reverse().ToArray());

        if(
            WordChecker.isWordValid(wordXN)||
            WordChecker.isWordValid(wordXR)||
            WordChecker.isWordValid(wordYN)||
            WordChecker.isWordValid(wordYR)
        ){
            // sadly emoji does not work in the unity console
            print("🎂🎉🎉🎉🎂🎂 WORD FOUND 🎂🎂🎂🎉🎂🎉🎉");
        }
    }

    /// <summary>
    /// Invokes a event telling all the letter displays to update their number of letters
    /// </summary>
    private void refreshLetterNumberDisplay(){
        LetterCountCangedEventArgs args = new LetterCountCangedEventArgs();
        args.AvailLetters = this.CurrentAvailableLetterCount;
        LetterCountCangedEvent?.Invoke(this,args);
    }


    /// <summary>
    /// Fills in the players letters
    /// </summary>
    /// <param name="args">the event args</param>
    private void FillPlayerLetters(LetterGameStartEventArgs args){
        Dictionary<string, int> playerDataDict = new Dictionary<string, int> {{"A",5},{"B",7},{"C",4}};// args.CurrentLetters; 
        foreach (string key in playerDataDict.Keys){
           if(playerLetters.Keys.Contains(key)){
               for (int i = 0; i < playerDataDict[key]; i++){
                    LetterGameLetter newLetter = new LetterGameLetter(-1,-1, key); 
                    playerLetters[key].Add(newLetter);
               }
               
           }
        }
    }

    /// <summary>
    /// Make a holder for every letter
    /// </summary>
    private void MakePlayerLetter(){
        foreach (String letter in letters)
        {
           this.playerLetters.Add(letter, new List<LetterGameLetter>());
        }
    }


    

    // -- tile map stuff -- //

    /// <summary>
    /// Tries to remove the provided letter from the board
    /// </summary>
    /// <param name="letter">the letter object to remove from the board</param>
    /// <returns>true if successful false if not</returns>
    private bool BoardTryRemoveLetter(LetterGameLetter letter){
        bool suc = false;
        if (letter.IsOnBoard){ // Fix WUArrays.MultiDimFind(tileMap, letter) and use it it's is more reliable
            if (letter != tileMap[letter.XPos,letter.YPos]){
                throw new Exception("TILE POSITION MISMATCH");
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
    
    /// <summary>
    /// Set a tile on the board. if the tile is occupied the occupant wil be removed
    /// </summary>
    /// <param name="x">the x pos to place the letter</param>
    /// <param name="y">the y pos to place the letter</param>
    /// <param name="tile">the letter objet to place</param>
    private void BoardSetTile(int x, int y, LetterGameLetter tile){
        LetterGameLetter oldTile = this.BoardTryGetTile(x,y);
        if (oldTile != null){
            print("removes old");
            // if there is a tile at the position remove it
            this.BoardTryRemoveLetter(oldTile);
        } 

        // remove the tiles old position from the log 
        if (tile.IsOnBoard){
            tileMap[tile.XPos,tile.YPos] = null;
        }
        
        
        tile.XPos = x;
        tile.YPos = y;
        tile.IsOnBoard = true;
        tileMap[x,y] = tile;
        //print($"New tile set at x{x}-y{y}");        
    }

    /// <summary>
    /// try to get the letter at the provided x and y pos
    /// returns the letter object if possible, null if not
    /// </summary>
    /// <param name="x">the x pos of the tile to get</param>
    /// <param name="y">the y pos of the tile to get</param>
    /// <returns></returns>
    private LetterGameLetter BoardTryGetTile(int x, int y){
        LetterGameLetter ret = null;

        if (BoardIsTileValid(x,y)){
            if(tileMap[x,y] != null){
                ret = tileMap[x,y];
            }
        }
        
        return ret;
    }

    /// <summary>
    /// checks if a tile on the board is valid 
    /// </summary>
    /// <param name="x">the x pos of the place to check</param>
    /// <param name="y">the y pos of the place to check</param>
    /// <returns>true if the tile is valid false if not</returns>
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
        this.tileMap = new LetterGameLetter[this.bSizeX,this.bSizeY];
        this.playerLetters = new Dictionary<String, List<LetterGameLetter>>();
        MakePlayerLetter();
        FillPlayerLetters(null);
        
        this.MakeBoardTiles();
        this.MakeLetterTile();

        

        refreshLetterNumberDisplay();
        
    }

}
