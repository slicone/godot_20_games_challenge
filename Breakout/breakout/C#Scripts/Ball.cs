using Godot;
using System;

public partial class Ball : CharacterBody2D
{
    [Export] public Area2D area2D { get; set; }
    [Export] public int BallSpeed { get; set; } = 100;
    public override void _Ready()
    {
        area2D.BodyEntered += BodyEntered;
        var angle = 65;
        var angleInRadiant = Mathf.DegToRad(angle);
        Velocity = new Vector2(BallSpeed * MathF.Cos(angleInRadiant), BallSpeed * MathF.Sin(angleInRadiant));

    }

    private void BodyEntered(Node2D body)
    {
        // will otherwise detect area collision
        if (body == this)
        {
            return;
        }

        if (body is Platform platform)
        {
            calculateNewBallAngle(platform);
            return;
        }

        if (body is VerticalWall)
        {
            Velocity = Velocity.Reflect(Vector2.Down);
        }

        if (body is HorizontalWall)
        {
            Velocity = Velocity.Reflect(Vector2.Right);
        }

    }

    private void calculateNewBallAngle(Platform platform)
    {
        var relativePlatformHit = (GlobalPosition.X - platform.GlobalPosition.X) / (platform.Sprite.Texture.GetWidth() / 2);
        var newAngle = Mathf.Lerp(60, platform.MaxDirectionAngle, relativePlatformHit);
        var angleInRadiant = Mathf.DegToRad(newAngle);
        Velocity = relativePlatformHit >= 0 ?
                        new Vector2(BallSpeed * MathF.Cos(angleInRadiant), BallSpeed * MathF.Sin(angleInRadiant) * -1) : // in godot > 0 is down and not up, because of that make it < 0
                        new Vector2(BallSpeed * MathF.Cos(angleInRadiant) * -1, BallSpeed * MathF.Sin(angleInRadiant) * -1); // direct in negative x, because plattform hit left part of platform 
    }


    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }


}
