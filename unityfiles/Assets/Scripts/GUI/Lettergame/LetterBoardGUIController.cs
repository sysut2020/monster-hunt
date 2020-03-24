using System.Collections.Generic;
using Helper;
using UnityEngine;
using UnityEngine.UI;

public class LetterBoardGUIController : MonoBehaviour {
    [SerializeField]
    [Tooltip("The letter tile prefab")]
    private GameObject letterTile;

    private GridLayoutGroup gridLayout;

    private CellSizeCalculator calculator;

    private readonly Dictionary<string, int> letters = new Dictionary<string, int> {
        {"A", 1},
        {"B", 3},
        {"C", 3},
        {"D", 2},
        {"E", 1},
        {"F", 4},
        {"G", 2},
        {"H", 4},
        {"I", 1},
        {"J", 8},
        {"K", 5},
        {"L", 1},
        {"M", 3},
        {"N", 1},
        {"O", 1},
        {"P", 3},
        {"Q", 10},
        {"R", 1},
        {"S", 1},
        {"T", 1},
        {"U", 1},
        {"V", 4},
        {"W", 4},
        {"X", 8},
        {"Y", 4},
        {"Z", 10}
    };

    private RectTransform rectTransform;

    void Start() {
        rectTransform = gameObject.GetComponent<RectTransform>();


        var columns = 13; // 13 columns for all the letters 26 / 2
        var rows = 2; // 2 rows for all the letters 26 / 13
        calculator = new CellSizeCalculator(rectTransform, rows, columns);
        
        gridLayout = gameObject.GetComponent<GridLayoutGroup>();
        // setting the cell size to match screen resolution
        gridLayout.cellSize = calculator.GetOptimalCellSize();

        MakeLetterTile();
    }

    /// <summary>
    /// Makes and places the letter tiles
    /// </summary>
    private void MakeLetterTile() {
        foreach (var letter in letters) {
            GameObject n = Instantiate(this.letterTile, transform, true);
            var nRect = n.GetComponent<RectTransform>();

            LetterTile tile = n.GetComponent<LetterTile>();
            tile.LetterTileLetter = letter.Key;
            nRect.localScale = new Vector3(1, 1, 1);
        }
    }
}