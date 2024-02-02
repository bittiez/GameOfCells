using GameOfCells;
using Godot;

public partial class main : Node2D
{
	public static Node2D MainSceneNode { get; private set; }

	private const double TIME_PER_GAME_STEP = 1;

	private double currentDelta = 0;

	public override void _EnterTree()
	{
		base._EnterTree();
		MainSceneNode = this;

		GridManager.Instance.GenerateCellNodes();
		GridManager.Instance.GenerateSemiRandomCells();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		currentDelta += delta;

		if (currentDelta >= TIME_PER_GAME_STEP)
		{
			currentDelta = 0;

			GridManager.Instance.ProcessStep();
		}
	}
}
