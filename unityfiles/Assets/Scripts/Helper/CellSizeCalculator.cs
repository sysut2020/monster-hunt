using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Helper {
    /// <summary>
    /// Helper to calculate a cell size in a grid layout
    /// </summary>
    public class CellSizeCalculator {
        private readonly RectTransform container;
        private readonly float rows;
        private readonly float columns;
        private readonly GridLayoutGroup gridLayout;
        private readonly RectOffset gridLayoutPadding;

        public CellSizeCalculator (RectTransform container, float rows, float columns) {
            this.container = container;
            this.rows = rows;
            this.columns = columns;

            gridLayout = container.GetComponent<GridLayoutGroup> ();
            if (gridLayout == null) {
                throw new InvalidDataException ("The container does not contain a GridLayoutGroup component");
            }
            gridLayoutPadding = gridLayout.padding;
        }

        /// <summary>
        /// Returns the optimal width of a cell in a grid layout group with a set number of columns
        /// </summary>
        /// <returns></returns>
        private int GetOptimalCellSizeWidth () {
            var rect = container.rect;
            float height = rect.height;
            float width = rect.width;

            int horizontalPadding = gridLayoutPadding.horizontal;
            int verticalPadding = gridLayoutPadding.vertical;

            var cellSizeX = Mathf.RoundToInt ((width - horizontalPadding) / columns);
            var cellSizeY = Mathf.RoundToInt ((height - verticalPadding) / rows);
            var cellSize = Math.Min (cellSizeX, cellSizeY);

            return cellSize;
        }

        /// <summary>
        /// Returns the optimal amount of rows for the container given to the calculator
        /// </summary>
        /// <returns></returns>
        public int GetOptimalAmountOfRows () {
            int verticalPadding = gridLayoutPadding.vertical;
            var cellSizeWidth = GetOptimalCellSizeWidth ();
            float height = container.rect.height;
            return Mathf.FloorToInt ((height - verticalPadding) / cellSizeWidth);
        }

        public Vector2 GetOptimalCellSize () {
            var cellWidth = GetOptimalCellSizeWidth ();
            var cellSize = gridLayout.cellSize;
            var cellOrgWidth = cellSize.x;
            var cellOrgHeight = cellSize.y;

            var cellOrgHeightToWidthRatio = cellOrgHeight / cellOrgWidth;
            var cellHeight = cellWidth * cellOrgHeightToWidthRatio;
            return new Vector2 (cellWidth, cellHeight);
        }
    }
}