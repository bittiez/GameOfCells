using GameOfCells;
using Godot;

public partial class Steps : Label
{
	private long lastStep = 0;
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (lastStep != GridManager.Instance.Steps)
		{
			Text = GridManager.Instance.Steps.ToString();
			lastStep = GridManager.Instance.Steps;
		}
	}
}
