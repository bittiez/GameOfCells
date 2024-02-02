using Godot;
using System.Diagnostics;

namespace GameOfCells
{
    internal class GridManager
    {
        public static GridManager Instance { get; private set; } = new GridManager();
        //Game window is 768x1024, so keep this in divisibles of that.
        public const int CELLSIZE = 16;

        public int GridWidth => gridMap.GetLength(0);
        public int GridHeight => gridMap.GetLength(1);

        public long Steps { get; private set; } = 0;
        public int CellCount { get; private set; } = 0;

        private Cell[,] gridMap;

        private GridManager()
        {
            gridMap = new Cell[768 / CELLSIZE, 1024 / CELLSIZE];
        }

        /// <summary>
        /// Get the cell at the grid location. Returns null if out of bounds.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Cell GetCell(int x, int y)
        {
            if (x >= 0 && x < gridMap.GetLength(0) && y >= 0 && y < gridMap.GetLength(1))
            {
                if (gridMap[x, y] == null)
                {
                    return gridMap[x, y] = new Cell(x, y);
                }

                return gridMap[x, y];
            }

            return null;
        }

        /// <summary>
        /// Should only be ran once, unless resetting cells.
        /// </summary>
        public void GenerateCellNodes()
        {
            for (int i = 0; i < gridMap.GetLength(0); i++)
            {
                for (int j = 0; j < gridMap.GetLength(1); j++)
                {
                    gridMap[i, j] = new Cell(i, j);
                }
            }
        }

        public void Reset()
        {
            Steps = 0;

            for (int i = 0; i < gridMap.GetLength(0); i++)
            {
                for (int j = 0; j < gridMap.GetLength(1); j++)
                {
                    gridMap[i, j].Type = Cell.CellType.None;
                }
            }

            GenerateSemiRandomCells();
        }

        /// <summary>
        /// Only run this **AFTER** GenerateCellNodes()
        /// </summary>
        public void GenerateSemiRandomCells()
        {
            int blocks = GD.RandRange(2, 4);

            for (int i = 0; i < blocks; i++)
            {
                int x = GD.RandRange(10, GridWidth - 10);
                int y = GD.RandRange(10, GridHeight - 10);

                //Change to randomized

                for (int j = -1; j <= 2; j++)
                {
                    for (int k = -1; k <= 2; k++)
                    {
                        if (GD.Randf() > 0.50)
                        {
                            GetCell(j + x, k + y).Type = Cell.CellType.Cell;

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process a step in the game
        /// </summary>
        public void ProcessStep()
        {
#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            bool hadAChange = false;
            int newCount = 0;

            for (int i = 0; i < gridMap.GetLength(0); i++)
            {
                for (int j = 0; j < gridMap.GetLength(1); j++)
                {
                    Cell cell = gridMap[i, j];
                    cell.ProcessStep();

                    if (cell.Type > Cell.CellType.Mutating)
                    {
                        ApplySpecialRules(cell);
                    }
                    else
                    {
                        int neighbors = CountNeighbors(i, j);
                        bool afteRulesApplied = ApplyRules(cell.IsCell, neighbors);
                        if (afteRulesApplied != cell.IsCell)
                        {
                            cell.Type = afteRulesApplied ? Cell.CellType.Cell : Cell.CellType.None;
                            hadAChange = true;
                        }
                    }

                    if (cell.IsCell) { newCount++; }
                }
            }

            if (hadAChange)
            {
                Steps++;
            }
            CellCount = newCount;

#if DEBUG
            sw.Stop();
            StepTimer.Instance.Text = $"{sw.Elapsed.TotalMilliseconds}ms";
#endif
        }

        /// <summary>
        /// Count neighboring cells for x,y position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CountNeighbors(int x, int y)
        {
            int count = 0;

            // Check the eight neighbors around the current cell
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int neighborX = x + i;
                    int neighborY = y + j;

                    // Skip the current cell
                    if (i == 0 && j == 0)
                        continue;

                    // Check boundaries and count live neighbors
                    if (neighborX >= 0 && neighborX < gridMap.GetLength(0) && neighborY >= 0 && neighborY < gridMap.GetLength(1))
                    {
                        if (gridMap[neighborX, neighborY].IsCell)
                            count++;
                    }
                }
            }

            return count;
        }

        private bool ApplyRules(bool currentState, int neighbors)
        {
            // Apply Conway's rules
            if (currentState)
            {
                // Any live cell with fewer than two live neighbors dies (underpopulation)
                // Any live cell with two or three live neighbors lives on to the next generation
                // Any live cell with more than three live neighbors dies (overpopulation)
                return neighbors == 2 || neighbors == 3;
            }
            else
            {
                // Any dead cell with exactly three live neighbors becomes a live cell (reproduction)
                return neighbors == 3;
            }
        }

        private void ApplySpecialRules(Cell cell)
        {
            if (cell.Type == Cell.CellType.Virus)
            {
                Cell[] neighbors = cell.GetNeighbors();
                Cell chosenCell = neighbors[GD.RandRange(0, neighbors.Length - 1)];
                if (chosenCell != null)
                {
                    chosenCell.Type = Cell.CellType.None;
                }

                if (GD.Randf() > 0.98)
                {
                    cell.Type = Cell.CellType.None;
                }
            }
        }
    }
}
