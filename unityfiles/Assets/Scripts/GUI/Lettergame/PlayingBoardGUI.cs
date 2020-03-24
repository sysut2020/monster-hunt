using Helper;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Lettergame {
    public class PlayingBoardGUI : MonoBehaviour {
        private RectTransform playboardCanvas;

        private float boardSizeX = 27;
        private float boardSizeY = 10;
        
        private GridLayoutGroup gridLayout;

        CellSizeCalculator calculator;

        [SerializeField]
        private GameObject boardTile;

        private int tempRows;

        private void Start() {
            gridLayout = gameObject.GetComponent<GridLayoutGroup>();
            playboardCanvas = gameObject.GetComponent<RectTransform>();

            calculator = new CellSizeCalculator(playboardCanvas, boardSizeY, boardSizeX);
            gridLayout.cellSize = calculator.GetOptimalCellSize();

            FillPlayingBoard();
        }

        void FillPlayingBoard() {
            var rows = calculator.GetOptimalAmountOfRows();
            var columns = boardSizeX;

            for (int i = 0; i < columns * rows; i++) {
                GameObject n = Instantiate(this.boardTile, transform, true);
                GameBoardTile tile = n.GetComponent<GameBoardTile>();
                var rectTransform = n.GetComponent<RectTransform>();

                tile.XPos = i % Mathf.RoundToInt(columns);
                tile.YPos = (int) Mathf.Floor(i / columns);
                rectTransform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}