using GameOfCells;
using Godot;

public partial class CellCount : Label
{
	private int lastCount = 0;
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (lastCount != GridManager.Instance.CellCount)
		{
			lastCount = GridManager.Instance.CellCount;
			Text = $"{lastCount} cells";
		}
	}
}
