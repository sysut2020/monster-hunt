using System;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Lettergame {
    public class PlayingBoardGUI : MonoBehaviour {
        private RectTransform playboardCanvas;

        private float boardSizeX = 27;
        private float boardSizeY = 10;

        private int orgBoardTileCellSize = 50;

        private GridLayoutGroup gridLayout;

        private int cellSizeX;
        private int cellSizeY;

        private int cellSize;

        private float scaleFactor;

        [SerializeField]
        private GameObject boardTile;

        private int tempRows; 
        private void Start() {
            gridLayout = gameObject.GetComponent<GridLayoutGroup>();
            playboardCanvas = gameObject.GetComponent<RectTransform>();
            var rect = playboardCanvas.rect;
            float height = rect.height;
            // float height = gridLayout;
            float width = rect.width;

            RectOffset gridLayoutPadding = gridLayout.padding;
            int horizontalPadding = gridLayoutPadding.horizontal;
            int verticalPadding = gridLayoutPadding.vertical;

            cellSizeX = Mathf.RoundToInt((width - horizontalPadding) / boardSizeX);
            cellSizeY = Mathf.RoundToInt((height - verticalPadding) / boardSizeY);

            cellSize = Math.Min(cellSizeX, cellSizeY);

            // scaleFactor = cellSize / (float) orgBoardTileCellSize;
            gridLayout.cellSize = new Vector2(cellSize, cellSize);

            tempRows = Mathf.FloorToInt((height - verticalPadding) / cellSize);

            FillPlayingBoard();
        }

        void FillPlayingBoard() {
            for (int i = 0; i <  
                            boardSizeX * tempRows
                ; i++) {
                GameObject n = Instantiate(this.boardTile);
                GameBoardTile tile = n.GetComponent<GameBoardTile>();
                var rectTransform = n.GetComponent<RectTransform>();
                var rect = rectTransform.rect;
                // rect.height = cellSize;
                // rect.width = cellSize;
                
                
                tile.XPos = i % Mathf.RoundToInt(boardSizeX);
                tile.YPos = (int) Mathf.Floor(i / boardSizeX);
                n.transform.SetParent(transform);
                rectTransform.localScale = new Vector3(1,1, 1);
            }
        }
    }
}