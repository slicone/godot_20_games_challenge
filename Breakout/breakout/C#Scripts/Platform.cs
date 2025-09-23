using Godot;

public partial class Platform : CharacterBody2D
{
    [Export] public float MoveSpeed { get; set; } = 7000;
            
    public override void _PhysicsProcess(double delta)
    {
        var moveSpeed = MoveSpeed * (float)delta;
        float movement = Input.GetAxis("move-left", "move-right") * moveSpeed;
        
        Velocity = movement != 0 ?
            // movement
            Velocity = new Vector2(movement, Velocity.Y) :
            // no movement
            Velocity = Vector2.Zero; 

        MoveAndSlide();
    }

}
