using Helper;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Lettergame {
    /// <summary>
    /// Responsible for creating the letter board and showing all the letters
    /// </summary>
    public class LetterBoardGUIController : MonoBehaviour {
        [SerializeField]
        [Tooltip ("The letter tile prefab")]
        private GameObject letterTile;

        void Start () {
            var columns = 13; // 13 columns for all the letters 26 / 2
            var rows = 2; // 2 rows for all the letters 26 / 13
            var rectTransform = gameObject.GetComponent<RectTransform> ();
            var calculator = new CellSizeCalculator (rectTransform, rows, columns);

            // setting the cell size to match screen resolution
            var gridLayout = gameObject.GetComponent<GridLayoutGroup> ();
            gridLayout.cellSize = calculator.GetOptimalCellSize ();

            MakeLetterTile ();
        }

        /// <summary>
        /// Makes and places the letter tiles
        /// </summary>
        private void MakeLetterTile () {
            var letters = LetterLevelController.Instance.Letters;

            foreach (var letter in letters) {
                GameObject n = Instantiate (this.letterTile, transform, true);
                var nRect = n.GetComponent<RectTransform> ();

                LetterTile tile = n.GetComponent<LetterTile> ();
                tile.LetterTileLetter = letter.Key;
                nRect.localScale = new Vector3 (1, 1, 1);
            }
        }
    }
}