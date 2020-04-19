using Helper;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Lettergame {

    /// <summary>
    /// Responsible for filling up the playing board with tiles
    /// </summary>
    public class PlayingBoardGUI : MonoBehaviour {

        private float boardSizeX;
        private float boardSizeY;

        private CellSizeCalculator calculator;

        [SerializeField]
        private GameObject boardTile;

        private void Start() {
            var letterGameController = LetterLevelController.Instance;
            boardSizeX = letterGameController.BSizeX;
            boardSizeY = letterGameController.BSizeY;

            var gridLayout = gameObject.GetComponent<GridLayoutGroup>();
            var playBoardCanvas = gameObject.GetComponent<RectTransform>();

            calculator = new CellSizeCalculator(playBoardCanvas, boardSizeY, boardSizeX);
            gridLayout.cellSize = calculator.GetOptimalCellSize();

            FillPlayingBoard();
        }

        private void FillPlayingBoard() {
            var rows = calculator.GetOptimalAmountOfRows();
            var columns = boardSizeX;

            for (int i = 0; i < columns * rows; i++) {
                GameObject n = Instantiate(this.boardTile, transform, true);
                GameBoardTile tile = n.GetComponent<GameBoardTile>();
                var rectTransform = n.GetComponent<RectTransform>();

                tile.XPos = i % Mathf.RoundToInt(columns);
                tile.YPos = (int)Mathf.Floor(i / columns);
                rectTransform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}