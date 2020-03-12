using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// -- properties -- //
// -- events -- // 
// -- public -- //
// -- private -- // 
// -- unity -- //

public class LetterGameStartEventArgs : EventArgs {
    public Dictionary<string, int> CurrentLetters { get; set; }
}

public class BoardChangedEventArgs : EventArgs {
    private LetterGameLetter[, ] TileMap { get; set; }
}

public class LetterCountCangedEventArgs : EventArgs {
    public Dictionary<string, int> AvailLetters { get; set; }
}

public class LetterGameManager : Singleton<LetterGameManager> {

    public bool backsearch = true;

    /// <summary>
    /// Dimension constants for multidimansional(2D) arrays
    /// Y = 0
    /// X = 1
    /// </summary>
    private const int YDIMENSION = 0;
    private const int XDIMENSION = 1;

    // maby move this to a global constant
    private readonly string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

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
    private LetterGameLetter[, ] tileMap;

    // -- properties -- //
    public Dictionary<string, int> CurrentAvailableLetterCount {
        get {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            foreach (string key in playerLetters.Keys) {
                ret.Add(key, 0);
                foreach (LetterGameLetter l in playerLetters[key]) {
                    if (!l.IsOnBoard) { ret[key] += 1; }
                }
            }
            return ret;
        }
    }
    // -- events -- // 
    public static event EventHandler<LetterCountCangedEventArgs> LetterCountCangedEvent;

    // -- public -- //

    /// <summary>
    /// try's to get a specific letter from the game manager
    /// If a letter is available (not on the board) the letters object is returned
    /// if not null
    /// </summary>
    /// <param name="letter">the letter to check if is available</param>
    /// <returns>the letters objet is it is available else null</returns>
    public LetterGameLetter TryGetLetter(string letter) {
        LetterGameLetter ret = null;
        foreach (LetterGameLetter l in playerLetters[letter]) {
            if (!l.IsOnBoard) {
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
    public void UpdateLetterPos(int newX, int newY, LetterGameLetter letter) {
        float timestamp = DateTime.Now.Millisecond * UnityEngine.Random.Range(0f, 100f);
        int prevX = letter.XPos;
        int prevY = letter.YPos;

        if (BoardIsTileValid(newX, newY)) {
            // check for new words from the new point
            this.BoardSetTile(newX, newY, letter);
            this.ChekIfWordFromPos(newX, newY, timestamp, true);
        } else {
            // a letter til has been removed from the screen remove it
            BoardTryRemoveLetter(letter);
        }

        if (prevX < 0 && prevY < 0) {
            prevX = newX;
            prevY = newY;
        }

        if (backsearch) {
            int prevPX = prevX + 1;
            if (BoardIsTileValid(prevPX, prevY) && !(prevPX == newX && prevY == newY)) {
                LetterGameLetter[] con = TryGetConnectedLetters(prevPX, prevY, XDIMENSION);
                if (con != null) {
                    this.TraverseInDirection(prevPX, prevY, timestamp, XDIMENSION, false, 0);
                    foreach (var item in con) {
                        TraverseInDirection(item.XPos, item.YPos, timestamp, YDIMENSION, true, 2);
                    }
                }
            }
            int prevNX = prevX - 1;
            if (BoardIsTileValid(prevNX, prevY) && !((prevNX == newX) && (prevY == newY))) {
                LetterGameLetter[] con = TryGetConnectedLetters(prevNX, prevY, XDIMENSION);
                if (con != null) {
                    this.TraverseInDirection(prevNX, prevY, timestamp, XDIMENSION, false, 0);
                    foreach (var item in con.Reverse()) {
                        TraverseInDirection(item.XPos, item.YPos, timestamp, YDIMENSION, true, 2);
                    }
                }
            }

            int prevPY = prevY + 1;
            if (BoardIsTileValid(prevX, prevPY) && !(prevX == newX && prevPY == newY)) {
                LetterGameLetter[] con = TryGetConnectedLetters(prevX, prevPY, YDIMENSION);
                if (con != null) {
                    this.TraverseInDirection(prevX, prevPY, timestamp, YDIMENSION, false, 0);
                    foreach (var item in con) {
                        TraverseInDirection(item.XPos, item.YPos, timestamp, XDIMENSION, true, 2);
                    }
                }
            }

            int prevNY = prevY - 1;
            if (BoardIsTileValid(prevX, prevNY) && !(prevX == newX && prevNY == newY)) {
                LetterGameLetter[] con = TryGetConnectedLetters(prevX, prevNY, YDIMENSION);
                if (con != null) {
                    this.TraverseInDirection(prevX, prevNY, timestamp, YDIMENSION, false, 0);
                    foreach (var item in con.Reverse()) {
                        TraverseInDirection(item.XPos, item.YPos, timestamp, XDIMENSION, true, 2);
                    }
                }
            }
        }

        this.refreshLetterNumberDisplay();
    }

    // -- private -- //

    /// <summary>
    /// Makes and places the board tiles
    /// </summary>
    private void MakeBoardTiles() {
        for (int i = 0; i < bSizeX * bSizeY; i++) {
            GameObject n = Instantiate(this.boardTile);
            GameBoardTile tile = n.GetComponent<GameBoardTile>();
            tile.XPos = i % bSizeX;
            tile.YPos = (int) Mathf.Floor(i / bSizeX);
            n.transform.SetParent(playingBoard.transform);
        }
    }

    /// <summary>
    /// Makes and places the letter tiles
    /// </summary>
    private void MakeLetterTile() {
        foreach (string letter in letters) {
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
    private void ChekIfWordFromPos(int x, int y, float timestamp, bool deepTraverse) {
        if (!BoardIsTileValid(x, y)) { return; }
        if (BoardTryGetTile(x, y) == null) { return; }

        var yConnected = WUArrays.GetConnected(tileMap, x, y, YDIMENSION);
        foreach (var item in yConnected) {
            if (deepTraverse) {
                if (item.isValidLetterInWord) {
                    TraverseInDirection(item.XPos, item.YPos, timestamp, XDIMENSION, true, 2);
                }
            }
        }

        var xConnected = WUArrays.GetConnected(tileMap, x, y, XDIMENSION);
        foreach (var item in xConnected) {
            if (deepTraverse) {
                if (item.isValidLetterInWord) {
                    TraverseInDirection(item.XPos, item.YPos, timestamp, YDIMENSION, true, 2);
                }
            }
        }

        var yDirection = yConnected.Select(tile => tile.Letter).ToArray();
        var xDirection = xConnected.Select(tile => tile.Letter).ToArray();

        bool isValidInY = false;
        bool isValidInX = false;

        string wordYN = string.Concat(yDirection.ToArray());

        string wordXN = string.Concat(xDirection.ToArray());

        if (WordChecker.isWordValid(wordYN)) {
            isValidInY = true;
        }

        if (WordChecker.isWordValid(wordXN)) {
            isValidInX = true;
        }

        foreach (var le in xConnected) {
            le.SetValidLetter(isValidInX, timestamp);
        }

        foreach (var le in yConnected) {
            le.SetValidLetter(isValidInY, timestamp);
        }

    }

    private LetterGameLetter[] TryGetConnectedLetters(int xpos, int ypos, int dimension) {
        return WUArrays.GetConnected(this.tileMap, xpos, ypos, dimension);
    }

    /// <summary>
    /// Checks if there are any words formed from drawing a vertical or horizontal line 
    /// through the adjacent letters 
    /// </summary>
    /// <param name="x">the x pos of the cord to check from</param>
    /// <param name="y">the x pos of the cord to check from</param>
    private void TraverseInDirection(int x, int y, float timestamp, int direction, bool deepTraverse, int levels) {
        if (!BoardIsTileValid(x, y)) { return; }
        if (BoardTryGetTile(x, y) == null) { return; }
        levels--;
        if (direction == 1) {
            string word = "X: ";
            var xConnected = TryGetConnectedLetters(x, y, XDIMENSION);
            bool chain = true;
            foreach (var item in xConnected) {
                if (deepTraverse && levels > 0) {
                    word += item.Letter;
                    if (item.isValidLetterInWord) {
                        var xSubConnected = TryGetConnectedLetters(item.XPos, item.YPos, YDIMENSION);
                        string subs = "SUB X: ";
                        foreach (var e in xSubConnected) {
                            subs += e.Letter;
                            if (!e.isValidLetterInWord) chain = false;
                        }
                        Debug.Log(subs);
                        if (chain) {
                            TraverseInDirection(item.XPos, item.YPos, timestamp, YDIMENSION, true, levels);
                        }
                    } 
                }
            }
            var xDirection = xConnected.Select(tile => tile.Letter).ToArray();
            string wordXN = string.Concat(xDirection.ToArray());
            bool isValidInX = false;
            if (WordChecker.isWordValid(wordXN)) {
                isValidInX = true;
            }
            Debug.Log(word);
            if (xConnected.Length > 1) {
                foreach (var le in xConnected) {
                    le.SetValidLetter(isValidInX, timestamp);
                }
            }

        } else {
            string word = "Y: ";
            var yConnected = TryGetConnectedLetters(x, y, YDIMENSION);
            bool chain = true;
            foreach (var item in yConnected) {
                if (deepTraverse && levels > 0) {
                    word += item.Letter;
                    if (item.isValidLetterInWord) {
                        var ySubConnected = TryGetConnectedLetters(item.XPos, item.YPos, XDIMENSION);
                        string subs = "SUB Y: ";
                        foreach (var e in ySubConnected) {
                            subs += e.Letter;
                            if (!e.isValidLetterInWord) chain = false;
                        }
                        Debug.Log(subs);
                        if (chain) {
                            TraverseInDirection(item.XPos, item.YPos, timestamp, XDIMENSION, true, levels);
                        }
                    }
                }
            }
            Debug.Log(word);
            if (yConnected.Length > 1) {
                var yDirection = yConnected.Select(tile => tile.Letter).ToArray();

                bool isValidInY = false;

                string wordYN = string.Concat(yDirection.ToArray());

                if (WordChecker.isWordValid(wordYN)) {
                    isValidInY = true;
                }
                foreach (var le in yConnected) {
                    le.SetValidLetter(isValidInY, timestamp);
                }
            }

        }

    }

    /// <summary>
    /// Invokes a event telling all the letter displays to update their number of letters
    /// </summary>
    private void refreshLetterNumberDisplay() {
        LetterCountCangedEventArgs args = new LetterCountCangedEventArgs();
        args.AvailLetters = this.CurrentAvailableLetterCount;
        LetterCountCangedEvent?.Invoke(this, args);
    }

    /// <summary>
    /// Fills in the players letters
    /// </summary>
    /// <param name="args">the event args</param>
    private void FillPlayerLetters(Dictionary<string, int> w) {
        Dictionary<string, int> playerDataDict = new Dictionary<string, int> { { "A", 25 },
            { "B", 25 },
            { "C", 25 },
            { "D", 25 },
            { "E", 25 },
            { "F", 25 },
            { "G", 25 },
            { "H", 25 },
            { "I", 25 },
            { "J", 25 },
            { "K", 25 },
            { "L", 25 },
            { "M", 25 },
            { "N", 25 },
            { "O", 25 },
            { "P", 25 },
            { "Q", 25 },
            { "R", 25 },
            { "S", 25 },
            { "T", 25 },
            { "U", 25 },
            { "V", 25 },
            { "W", 25 },
            { "X", 25 },
            { "Y", 25 },
            { "Z", 25 }
        }; // when communication from GM is in use: args.CurrentLetters; 
        foreach (string key in playerDataDict.Keys) {
            if (playerLetters.Keys.Contains(key)) {
                for (int i = 0; i < playerDataDict[key]; i++) {
                    LetterGameLetter newLetter = new LetterGameLetter(-1, -1, key);
                    playerLetters[key].Add(newLetter);
                }

            }
        }
    }

    /// <summary>
    /// Make a holder for every letter
    /// </summary>
    private void MakePlayerLetter() {
        foreach (String letter in letters) {
            this.playerLetters.Add(letter, new List<LetterGameLetter>());
        }
    }

    // -- tile map stuff -- //

    /// <summary>
    /// Tries to remove the provided letter from the board
    /// </summary>
    /// <param name="letter">the letter object to remove from the board</param>
    /// <returns>true if successful false if not</returns>
    private bool BoardTryRemoveLetter(LetterGameLetter letter) {
        bool suc = false;
        var foundLetter = WUArrays.MultiDimFind(tileMap, letter);
        if (foundLetter != null) {
            tileMap[letter.XPos, letter.YPos] = null;
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
    private void BoardSetTile(int x, int y, LetterGameLetter tile) {
        LetterGameLetter oldTile = this.BoardTryGetTile(x, y);
        if (oldTile != null) {
            // if there is a tile at the position remove it
            this.BoardTryRemoveLetter(oldTile);
        }

        // remove the tiles old position from the log 
        if (tile.IsOnBoard) {
            tileMap[tile.XPos, tile.YPos] = null;
        }

        tile.XPos = x;
        tile.YPos = y;
        tile.IsOnBoard = true;
        tileMap[x, y] = tile;
    }

    /// <summary>
    /// try to get the letter at the provided x and y pos
    /// returns the letter object if possible, null if not
    /// </summary>
    /// <param name="x">the x pos of the tile to get</param>
    /// <param name="y">the y pos of the tile to get</param>
    /// <returns></returns>
    private LetterGameLetter BoardTryGetTile(int x, int y) {
        LetterGameLetter ret = null;

        if (BoardIsTileValid(x, y)) {
            if (tileMap[x, y] != null) {
                ret = tileMap[x, y];
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
    private bool BoardIsTileValid(int x, int y) {
        bool ret = true;
        if (
            x > tileMap.GetUpperBound(0) ||
            x < tileMap.GetLowerBound(0) ||
            y > tileMap.GetUpperBound(1) ||
            y < tileMap.GetLowerBound(1)
        ) { ret = false; }
        return ret;
    }

    // Start is called before the first frame update
    void Start() {
        this.tileMap = new LetterGameLetter[this.bSizeX, this.bSizeY];
        this.playerLetters = new Dictionary<String, List<LetterGameLetter>>();
        MakePlayerLetter();
        FillPlayerLetters(GameManager.Instance.PlayerPersistentStorage.AvailableLetters);

        this.MakeBoardTiles();
        this.MakeLetterTile();

        refreshLetterNumberDisplay();

    }

}