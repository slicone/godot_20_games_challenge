using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public StateMachine StateMachine { get; set; }
	[Export] public HitboxComponent HitboxComponent { get; set; }
	[Export] public HealthComponent HealthComponent { get; set; }
	[Export] public AnimationTreeHandler animationTreeHandler { get; set; }
	[Export] public float MoveSpeed { get; set; } = 200f;
	[Export] public float StopSpeed { get; set; } = 50f;
	[Export] public float JumpVelocity { get; set; } = -200.0f;
	[Export] public float FallVelocity { get; set; } = -9.81f;

	[Export] public AbstractWand Wand { get; set; }

	[Signal] public delegate void PlayerDiedEventHandler();
	[Signal] public delegate void AttackEventHandler();
	[Signal] public delegate void InteractEventHandler();
	[Signal] public delegate void DropEventHandler();

	public override void _Ready()
	{
		if (HealthComponent is not null)
			HealthComponent.EntityDied += OnPlayerDied;

		if (HitboxComponent is not null && HealthComponent is not null)
			HitboxComponent.HealthComponent = HealthComponent;

		if (animationTreeHandler is null)
			GD.PrintErr("Animation Tree Handler dependency missing in player");

		StateMachine.Init(this);
	}


	private void OnPlayerDied()
    {
        
    }

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		StateMachine.ProcessInput(inputEvent);
	}

	public override void _Process(double delta)
	{
		StateMachine.ProcessFrame(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		StateMachine.ProcessPhysics(delta);
		var direction = Input.GetAxis("move-left", "move-right");
		animationTreeHandler.UpdatePlayerBlendPosition(direction);
	}
	
	

	public void SetVelocityOnMovement(float movement)
	{
		if (movement != 0)
		{
			Velocity = new Vector2(movement, Velocity.Y);

		}
		else
		{
			Velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, StopSpeed), Velocity.Y);
        }
	}

	public void ApplyGravityVelocity()
    {
		Velocity = new Vector2(Velocity.X, Velocity.Y - FallVelocity);	
    }

	public void TriggerAttack() => EmitSignal(SignalName.Attack);
	public void TriggerInteract() => EmitSignal(SignalName.Interact);
	public void TriggerDrop() => EmitSignal(SignalName.Drop);
}
