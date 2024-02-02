using Godot;
using System.Collections.Generic;

namespace GameOfCells
{
    public class Cell
    {
        private CellType type = CellType.None;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            CellNode = new CellNode(GridManager.CELLSIZE, GridManager.CELLSIZE, x, y);
            main.MainSceneNode.AddChild(CellNode);
        }

        public CellNode CellNode { get; private set; }
        public int X { get; }
        public int Y { get; }
        public bool IsCell => Type != CellType.None;

        public CellType Type
        {
            get => type; set
            {
                type = value;
                CellNode?.SetCell(type != CellType.None, value);
            }
        }

        public void ProcessStep()
        {
            if (GD.Randf() > 0.99)
            {
                switch (Type)
                {
                    case CellType.None:
                        break;
                    case CellType.Cell:
                        Type = CellType.Mutating;
                        break;
                    case CellType.Mutating:
                        Type = CellType.Virus;
                        break;
                    case CellType.Virus:
                        break;
                }
            }
        }

        /// <summary>
        /// Return neighboring cells
        /// </summary>
        /// <returns></returns>
        public Cell[] GetNeighbors()
        {
            List<Cell> neighbors = new List<Cell>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int neighborX = X + i;
                    int neighborY = Y + j;

                    // Skip the current cell
                    if (i == 0 && j == 0)
                        continue;

                    // Check boundaries and count live neighbors
                    if (neighborX >= 0 && neighborX < GridManager.Instance.GridWidth && neighborY >= 0 && neighborY < GridManager.Instance.GridHeight)
                    {
                        Cell cell = GridManager.Instance.GetCell(neighborX, neighborY);
                        if (cell.IsCell)
                            neighbors.Add(cell);
                    }
                }
            }

            return neighbors.ToArray();
        }

        public enum CellType
        {
            None,
            Cell,
            Mutating,
            Virus
        }
    }
}
