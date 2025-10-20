using Godot;
using System;

public partial class MoveState : State
{
	[Export] public State IdleState { get; set; }
	[Export] public State FallState { get; set; }
	[Export] public State JumpState { get; set; }

	public override void Enter()
	{
		Parent.animationTreeHandler?.SetAnimationTreeParameter("parameters/BasicMovement/movement/transition_request", "run");
	}

	public override State ProcessInput(InputEvent @event)
	{

		if (Input.IsActionJustPressed("jump") && Parent.IsOnFloor())
			return JumpState;

		return null;
	}

	public override State ProcessPhysics(double delta)
	{
		var movement = Input.GetAxis("move-left", "move-right") * Parent.MoveSpeed;

		if (!Parent.IsOnFloor())
			return FallState;


		Parent.SetVelocityOnMovement(movement);

		if (Parent.Velocity == Vector2.Zero)
		{
			return IdleState;
		}

		Parent.MoveAndSlide();
		return null;
	}

	
}
