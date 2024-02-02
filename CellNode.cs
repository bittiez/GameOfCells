using Godot;
using static GameOfCells.Cell;

namespace GameOfCells
{
    public partial class CellNode : Sprite2D
    {
        private static readonly Color EmptyColor = new Color(0, 0.176f, 0.29f);
        private static readonly Color CellColor = new Color(0.2f, 0.45f, 0.74f);
        private static readonly Color MutatingColor = new Color(0.4f, 0.45f, 0.74f);
        private static readonly Color VirusColor = new Color(0.8f, 0.45f, 0.74f);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">Size is 1px, this is scale technically</param>
        /// <param name="height">Size is 1px, this is scale technically</param>
        public CellNode(int width, int height, int x, int y)
        {
            Position = new Vector2(x * GridManager.CELLSIZE, y * GridManager.CELLSIZE);

            Texture = (Texture2D)GD.Load("res://assets/images/WhitePixel.png");
            Scale = new Vector2(width, height);
            Modulate = EmptyColor;
            Centered = false;
        }

        public override void _Ready()
        {
            base._Ready();
        }

        /// <summary>
        /// Set weather this sprite has a cell or is empty
        /// </summary>
        /// <param name="isCell"></param>
        public void SetCell(bool isCell, CellType type = CellType.None)
        {
            if (isCell)
            {
                switch (type)
                {
                    case CellType.None:
                        Modulate = EmptyColor;
                        break;
                    case CellType.Cell:
                        Modulate = CellColor;
                        break;
                    case CellType.Mutating:
                        Modulate = MutatingColor;
                        break;
                    case CellType.Virus:
                        Modulate = VirusColor;
                        break;
                }
            }
            else
            {
                Modulate = EmptyColor;
            }
        }

        public override void _ExitTree()
        {
            base._ExitTree();
        }
    }
}
