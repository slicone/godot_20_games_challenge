using Godot;

public partial class IdleState : State
{
	[Export] public State FallState { get; set; }
	[Export] public State JumpState { get; set; }
	[Export] public State MoveState { get; set; }

	public override void Enter()
    {
		Parent.animationTreeHandler?.SetAnimationTreeParameter("parameters/BasicMovement/movement/transition_request", "idle");
    }

	public override State ProcessInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("jump") && Parent.IsOnFloor())
			return JumpState;

		return null;
	}

	public override State ProcessPhysics(double delta)
	{
		float movement = Input.GetAxis("move-left", "move-right") * Parent.MoveSpeed;	

		if (!Parent.IsOnFloor())
			return FallState;


		Parent.SetVelocityOnMovement(movement);

		if (movement != 0)
			return MoveState;

		return null;
	}
}
