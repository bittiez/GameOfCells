using GameOfCells;
using Godot;

public partial class HighScoreCellCount : Label
{
	private long lastCells = 0;
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (lastCells != SaveManager.Instance.HighScoreSave.Cells)
		{
			lastCells = SaveManager.Instance.HighScoreSave.Cells;
			Text = $"Highest: {lastCells}";
		}
	}
}
