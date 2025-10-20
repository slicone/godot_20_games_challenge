using Godot;

public partial class FallState : State
{
	[Export] public State IdleState { get; set; }
	[Export] public State MoveState { get; set; }

    public override void Enter()
    {
		Parent.animationTreeHandler?.SetAnimationTreeParameter("parameters/BasicMovement/movement/transition_request", "fall");
    }

	public override State ProcessInput(InputEvent @event)
	{
		return null;
	}

	public override State ProcessPhysics(double delta)
	{
		Parent.ApplyGravityVelocity();

		float movement = Input.GetAxis("move-left", "move-right") * Parent.MoveSpeed;
		Parent.SetVelocityOnMovement(movement);
		Parent.MoveAndSlide();

		if (Parent.IsOnFloor())
		{
			if (Parent.Velocity != Vector2.Zero)
				return MoveState;
			return IdleState;
		}

		return null;
	}
}
