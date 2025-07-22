using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public Collision Collision { get; set; }
    public const int JUMP_VELOCITY = 300;
    public const int BASE_X_VELOCITY = 50;

    public override void _Ready()
    {
        Velocity += new Vector2(BASE_X_VELOCITY, 0);
        if (Collision is null)
        {
            GD.PushError("Player missing area collison dependency");
            return;
        }
        Collision.Init(this);
    }

    private void HandleUserInput()
    {
        if (Input.IsActionJustPressed("jump"))
            Velocity -= new Vector2(Velocity.X, JUMP_VELOCITY);
    }
   
    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float)delta;
            // base velocity x
            Velocity = new Vector2(BASE_X_VELOCITY, Velocity.Y);
        }

        HandleUserInput();
        MoveAndSlide();
    }

}
