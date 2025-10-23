using Godot;

public partial class Projectile : Area2D
{
    private Vector2 direction = Vector2.Right;
    private Vector2 velocity = Vector2.Zero;
    private float acceleration = 30;
    private Attack attack;

    public void Init(Vector2 direction, Attack attack)
    {
        this.direction = direction;
        this.attack = attack;        
    }

    public override void _Ready()
    {
        AreaEntered += CollidedWithArea;
        BodyEntered += CollidedWithBody;
    }

    public override void _PhysicsProcess(double delta)
    {
        velocity += direction * acceleration * (float)delta;
        Position += velocity;
    }

    private void CollidedWithArea(Area2D area)
    {
        if(area is HitboxComponent hitbox)
        {
            hitbox.Damage(attack);
        }
        QueueFree();
    }

    private void CollidedWithBody(Node2D body)
    {
        if(body is not Wand || body is not Projectile)
        {
            QueueFree();
        }
    }
}
