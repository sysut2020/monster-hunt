using System.Collections;
using System.Collections.Generic;
using Helper;
using UnityEngine;
using UnityEngine.UI;

public class LetterBoardGUIController : MonoBehaviour {
    [SerializeField]
    [Tooltip("The letter tile prefab")]
    private GameObject letterTile;
    
    private float cellOrgHeight = 53;
    private float cellOrgWidth = 50;

    private float cellOrgHeightToWidthRatio;

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

    // Start is called before the first frame update
    void Start() {
        rectTransform = gameObject.GetComponent<RectTransform>();
        
        calculator = new CellSizeCalculator(rectTransform, 2, 13);

        var cellWidth = calculator.GetOptimalCellSizeWidth();

        cellOrgHeightToWidthRatio = cellOrgHeight / cellOrgWidth;
        gridLayout = gameObject.GetComponent<GridLayoutGroup>();

        var cellHeight = cellWidth * cellOrgHeightToWidthRatio;
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);

        MakeLetterTile();
    }

    /// <summary>
    /// Makes and places the letter tiles
    /// </summary>
    private void MakeLetterTile() {
        foreach (var letter in letters) {
            GameObject n = Instantiate(this.letterTile);
            var rectTransform = n.GetComponent<RectTransform>();

            LetterTile tile = n.GetComponent<LetterTile>();
            tile.LetterTileLetter = letter.Key;
            n.transform.SetParent(transform);
            rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }
}