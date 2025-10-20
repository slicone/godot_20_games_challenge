using Godot;
using System;

public partial class State : Node
{
	[Export]
	public string AnimationName { get; set; }

	[Export]
	public Sprite2D Animation { get; set; }

	public Player Parent { get; set; }

	public virtual void Enter()
    {
		if (Parent is null)
			GD.PrintErr($"State {this} missing parent dependency");
    }

	public virtual void Exit() {}

	public virtual State ProcessInput(InputEvent inputEvent)
	{
		return null;
	}

	public virtual State ProcessFrame(double delta)
	{
		return null;
	}

	public virtual State ProcessPhysics(double delta)
	{
		return null;
	}

}
