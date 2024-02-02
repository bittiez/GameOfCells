using GameOfCells;
using Godot;

public partial class ColorRect : Godot.ColorRect
{
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event is InputEventMouseButton mouseEvent && @event.IsActionReleased("MouseClick"))
		{
			// Check if the click occurred within the node's bounding box
			if (GetGlobalRect().HasPoint(mouseEvent.GlobalPosition))
			{
				GetViewport().SetInputAsHandled();
				GridManager.Instance.Reset();
			}
		}
	}
}
