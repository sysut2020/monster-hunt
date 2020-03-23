using System;
using System.Collections.Generic;
using System.Linq;
using Monsterhunt.Fileoperation;
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

    private int wordsPoints = 0;

    private WordChecker wordChecker;

    /// <summary>
    /// Dimension constants for multidimansional(2D) arrays
    /// Y = 1
    /// X = 0
    /// </summary>
    private const int YDIMENSION = 1;
    private const int XDIMENSION = 0;

    private readonly Dictionary<string, int> letters = new Dictionary<string, int> { { "A", 1 },
        { "B", 3 },
        { "C", 3 },
        { "D", 2 },
        { "E", 1 },
        { "F", 4 },
        { "G", 2 },
        { "H", 4 },
        { "I", 1 },
        { "J", 8 },
        { "K", 5 },
        { "L", 1 },
        { "M", 3 },
        { "N", 1 },
        { "O", 1 },
        { "P", 3 },
        { "Q", 10 },
        { "R", 1 },
        { "S", 1 },
        { "T", 1 },
        { "U", 1 },
        { "V", 4 },
        { "W", 4 },
        { "X", 8 },
        { "Y", 4 },
        { "Z", 10 }
    };

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

    [Header("Testing")]
    [SerializeField]
    /// <summary>
    /// True if should generate test letters, if testing/debugging the scene
    /// </summary>
    private bool fillWithTestLetters = false;

    private Dictionary<String, List<LetterGameLetter>> playerLetters;
    private LetterGameLetter[, ] tileMap;

    // -- properties -- //
    public Dictionary<string, int> CurrentAvailableLetterCount {
        get {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            foreach (string key in playerLetters.Keys) {
                ret.Add(key, 0);
                foreach (LetterGameLetter l in playerLetters[key]) {
                    if (!l.IsOnBoard) { ret[key]++; }
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
        this.RefreshLetterCountDisplay();
        return ret;
    }

    /// <summary>
    /// Updates the provided letters position to the provided coordinates
    /// </summary>
    /// <param name="newX">the new x pos for the letter</param>
    /// <param name="newY">the new x pos for the letter</param>
    /// <param name="letter">the letter object to update the cords of</param>
    public void UpdateLetterPos(int newX, int newY, LetterGameLetter letter) {
        if (IsBoardTileValid(newX, newY)) {
            // Add the tile to the board
            this.BoardSetTile(newX, newY, letter);
        } else {
            // Remove the letter from the board
            BoardTryRemoveLetter(letter);
        }
        wordsPoints = 0;
        this.ResetAllTilesOnBoard();
        this.FindWordsInDimension(XDIMENSION);
        this.FindWordsInDimension(YDIMENSION);
        this.RefreshLetterCountDisplay();
        Debug.Log(wordsPoints);
    }

    /// <summary>
    /// Resets all the tiles on the board
    /// </summary>
    private void ResetAllTilesOnBoard() {
        foreach (var item in tileMap) {
            item?.SetValidLetter(false, Direction.CENTER);
        }
    }

    /// <summary>
    /// Finds words on the board, and builds the word when it 
    /// finds valid words in the provided dimension.
    /// </summary>
    /// <param name="dimension">dimension to search in</param>
    private void FindWordsInDimension(int dimension) {
        int minDimension1;
        int maxDimension1;
        int minDimension2;
        int maxDimension2;
        bool isXdimension = dimension == XDIMENSION;

        /*
            This block flips the X and Y depending on the dimension
            we want to search in
        */
        if (dimension == YDIMENSION) {
            minDimension1 = tileMap.GetLowerBound(XDIMENSION);
            maxDimension1 = tileMap.GetUpperBound(XDIMENSION);
            minDimension2 = tileMap.GetLowerBound(YDIMENSION);
            maxDimension2 = tileMap.GetUpperBound(YDIMENSION);
        } else {
            minDimension1 = tileMap.GetLowerBound(YDIMENSION);
            maxDimension1 = tileMap.GetUpperBound(YDIMENSION);
            minDimension2 = tileMap.GetLowerBound(XDIMENSION);
            maxDimension2 = tileMap.GetUpperBound(XDIMENSION);
        }

        for (int dimension1Counter = minDimension1; dimension1Counter <= maxDimension1; dimension1Counter++) {
            int dimension2Counter = minDimension2;
            // This is a fallback if the loop becomes trapped as a result of dim2counter boing set weird..
            int failproofer = 0;
            while (dimension2Counter <= maxDimension2 && failproofer <= maxDimension2) {
                // Since we flip X and Y based on dimension we search in, we have to
                // flip provided field to the tryfindword
                if (isXdimension) {
                    dimension2Counter = TryFindWordAtPosition(dimension2Counter, dimension1Counter, dimension);
                } else {
                    dimension2Counter = TryFindWordAtPosition(dimension1Counter, dimension2Counter, dimension);
                }
                dimension2Counter++;
                // Failproof counter.. since we modify dimensionCounter based on positions
                // it can cause infinite loops, if not careful. :D 
                failproofer++;
            }
        }
    }

    /// <summary>
    /// Tries to find a word in the position in the provided dimension X or Y 
    /// and return the X or Y position of the last letter in the word.
    /// If finding word in Y dimension, return Y position of last letter, if no 
    /// words are found, return the X or Y of the provided X Y.
    /// Updates the totalScore
    /// </summary>
    /// <param name="x">X position to search from</param>
    /// <param name="y">Y position to search from</param>
    /// <param name="dimension">the dimension to search in X or Y (0 or 1)</param>
    /// <returns>returns last letter position X ofr x dimension, Y for y dimension</returns>
    private int TryFindWordAtPosition(int x, int y, int dimension) {
        int lastposition = 0;
        if (IsBoardTileValid(x, y)) {
            var connectedLetters = TryGetConnectedLetters(x, y, dimension);
            if (CreateWordOfLetters(connectedLetters, dimension)) {
                wordsPoints += GetWordScore(connectedLetters);
                var pos = GetLastLetterPosition(connectedLetters);
                lastposition = (dimension == XDIMENSION) ? pos.x : pos.y;
            }
        }
        if (lastposition == 0) {
            lastposition = (dimension == XDIMENSION) ? x : y;
        }

        return lastposition;
    }

    /// <summary>
    /// Returns the score of connected letters/ a word
    /// </summary>
    /// <param name="word">the word to get points fron</param>
    /// <returns>total points for the word</returns>
    private int GetWordScore(LetterGameLetter[] word) {
        var sum = word.Sum(e => e.ScoreValue);
        return sum;
    }

    /// <summary>
    /// Returns the last letter position of connected letters.
    /// </summary>
    /// <param name="connectedLetters">array of connected letters</param>
    /// <returns>position of last connected letters</returns>
    private Point GetLastLetterPosition(LetterGameLetter[] connectedLetters) {
        Point lastPosition = new Point();
        if (connectedLetters != null) {
            int lastIndex = connectedLetters.Length - 1;
            var lastLetter = connectedLetters[Mathf.Clamp(lastIndex, 0, lastIndex)];
            lastPosition.x = lastLetter.XPos;
            lastPosition.y = lastLetter.YPos;
        }
        return lastPosition;
    }

    /// <summary>
    /// Checks if the connected letters is a valid word, and marks them as
    /// valid if they are part of a whole valid word. Returns true if 
    /// it created a word, else false.
    /// </summary>
    /// <param name="connectedLetters">letters to create word of</param>
    /// <returns>true if created word, else false</returns>
    private bool CreateWordOfLetters(LetterGameLetter[] connectedLetters, int direction) {
        bool createdWord = false;
        if (IsConnectedLetterValid(connectedLetters)) {
            foreach (var letter in connectedLetters) {
                var validDirection = direction == XDIMENSION ? Direction.RIGHT : Direction.DOWN;
                letter.SetValidLetter(true, validDirection);
            }
            createdWord = true;
        }
        return createdWord;
    }

    /// <summary>
    /// Checks if there are any words formed from drawing a vertical or horizontal line 
    /// through the adjacent letters 
    /// </summary>
    /// <param name="x">the x pos of the cord to check from</param>
    /// <param name="y">the x pos of the cord to check from</param>
    private bool IsConnectedLetterValid(LetterGameLetter[] connectedLetters) {
        bool isValidConnection = false;
        if (connectedLetters != null) {
            string word = GetWordStringOfLetters(connectedLetters);
            isValidConnection = wordChecker.isWordValid(word);
        }
        return isValidConnection;
    }

    /// <summary>
    /// Creates a word string of an array of letters
    /// </summary>
    /// <param name="connectedLetters">array of letters to convert to single string</param>
    /// <returns>string of the individual letters</returns>
    private string GetWordStringOfLetters(LetterGameLetter[] connectedLetters) {
        if (connectedLetters == null) return "";
        var letters = connectedLetters.Select(tile => tile.Letter).ToArray();
        return string.Concat(letters.ToArray());

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
        foreach (var letter in letters) {
            GameObject n = Instantiate(this.letterTile);
            LetterTile tile = n.GetComponent<LetterTile>();
            tile.LetterTileLetter = letter.Key;
            n.transform.SetParent(letterBoard.transform);
        }
    }

    private LetterGameLetter[] TryGetConnectedLetters(int xpos, int ypos, int dimension) {
        return WUArrays.GetConnected(this.tileMap, xpos, ypos, dimension);
    }

    /// <summary>
    /// Invokes a event telling all the letter displays to update their number of letters
    /// </summary>
    private void RefreshLetterCountDisplay() {
        LetterCountCangedEventArgs args = new LetterCountCangedEventArgs {
            AvailLetters = this.CurrentAvailableLetterCount
        };
        LetterCountCangedEvent?.Invoke(this, args);
    }

    /// <summary>
    /// Fills in the players letters
    /// </summary>
    /// <param name="args">the event args</param>
    private void FillPlayerLetters(Dictionary<string, int> playerDataDict) {
        if (playerDataDict == null) {
            throw new NullReferenceException("Letter dictionary is null");
        }
        foreach (string key in playerDataDict.Keys) {
            if (playerLetters.Keys.Contains(key)) {
                for (int i = 0; i < playerDataDict[key]; i++) {
                    int letterValue = letters[key];
                    LetterGameLetter newLetter = new LetterGameLetter(-1, -1, key, letterValue);
                    playerLetters[key].Add(newLetter);
                }

            }
        }
    }

    /// <summary>
    /// Make a holder for every letter
    /// </summary>
    private void MakePlayerLetter() {
        foreach (var letter in letters) {
            this.playerLetters.Add(letter.Key, new List<LetterGameLetter>());
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

            this.RefreshLetterCountDisplay();
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

        if (IsBoardTileValid(x, y)) {
            if (tileMap[x, y] != null) {
                ret = tileMap[x, y];
            }
        }

        return ret;
    }

    /// <summary>
    /// Check if the tile position is a valid position
    /// on the board.
    /// </summary>
    /// <param name="x">the x pos of the place to check</param>
    /// <param name="y">the y pos of the place to check</param>
    /// <returns>true if the tile is valid false if not</returns>
    private bool IsBoardTileValid(int x, int y) {
        bool valid = true;
        if (
            x > tileMap.GetUpperBound(0) ||
            x < tileMap.GetLowerBound(0) ||
            y > tileMap.GetUpperBound(1) ||
            y < tileMap.GetLowerBound(1)
        ) { valid = false; }
        return valid;
    }

    // Start is called before the first frame update
    void Start() {
        this.tileMap = new LetterGameLetter[this.bSizeX, this.bSizeY];
        this.playerLetters = new Dictionary<String, List<LetterGameLetter>>();
        MakePlayerLetter();

        if (this.fillWithTestLetters) {
            DebugFillWithLetters();
        } else {
            FillPlayerLetters(GameManager.Instance?.GameDataManager.PlayerLetters);
        }

        var fc = new FileReader("Assets/Resources/wordlist.txt");
        this.wordChecker = new WordChecker(fc.ReadAllLines().AsArray(), false);

        this.MakeBoardTiles();
        this.MakeLetterTile();
        RefreshLetterCountDisplay();

    }

    /// <summary>
    /// Fills the available letters board with letters when testing the letter
    /// game and you need letters to test words :D 
    /// </summary>
    private void DebugFillWithLetters() {
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
        };
        this.FillPlayerLetters(playerDataDict);
    }

}