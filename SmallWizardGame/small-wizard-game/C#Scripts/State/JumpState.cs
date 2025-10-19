using Godot;
using System;

public partial class JumpState : State
{
	[Export] 
	public State IdleState { get; set; }
	[Export] 
	public State MoveState { get; set; }
	[Export] 
	public State FallState { get; set; }


    public override void Enter()
    {
        // Apply vertical jump velocity
		Parent.Velocity = new Vector2(Parent.Velocity.X, Parent.JumpVelocity);
    }


	public override State ProcessInput(InputEvent @event)
	{
		CheckNonStateInput();
		return null;
	}

	public override State ProcessPhysics(double delta)
	{
		if(Parent.Velocity.Y > 0)
        {
			return FallState;
        }

		Parent.ApplyGravityVelocity();

		float movement = Input.GetAxis("move-left", "move-right") * Parent.MoveSpeed;
		Parent.SetVelocityOnMovement(movement);
		Parent.MoveAndSlide();
		

		return null;
	}
}