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
        var letters = LetterGameManager.Instance.Letters;

        foreach (var letter in letters) {
            GameObject n = Instantiate(this.letterTile, transform, true);
            var nRect = n.GetComponent<RectTransform>();

            LetterTile tile = n.GetComponent<LetterTile>();
            tile.LetterTileLetter = letter.Key;
            nRect.localScale = new Vector3(1, 1, 1);
        }
    }
}