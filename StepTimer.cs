using Godot;

public partial class StepTimer : Label
{
	public static StepTimer Instance;

	public override void _Ready()
	{
		base._Ready();
		Instance = this;
	}
}
