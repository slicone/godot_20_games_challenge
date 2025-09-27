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
            CalculateNewBallAngle(platform);
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

        if (body is Brick brick)
        {
            Velocity = Velocity.Reflect(Vector2.Right);
            brick.QueueFree();   
        }

    }

    private void CalculateNewBallAngle(Platform platform)
    {
        var relativePlatformHit = (GlobalPosition.X - platform.GlobalPosition.X) / (platform.Sprite.Texture.GetWidth() / 2);
        if (relativePlatformHit < 0) {
            // shift range of relativePlatformHit values between 0 and 1
            // otherwise angle will be under the minimal reflective angle
            var newAngle = Mathf.Lerp(platform.MaxDirectionAngle, platform.MinDirectionAngle, relativePlatformHit * -1);
            var angleInRadiant = Mathf.DegToRad(newAngle);
            Velocity = new Vector2(BallSpeed * MathF.Cos(angleInRadiant) * -1, BallSpeed * MathF.Sin(angleInRadiant) * -1); // direct in negative x, because plattform hit left part of platform 
        } else {
            var newAngle = Mathf.Lerp(platform.MaxDirectionAngle, platform.MinDirectionAngle, relativePlatformHit);
            var angleInRadiant = Mathf.DegToRad(newAngle);
            Velocity = new Vector2(BallSpeed * MathF.Cos(angleInRadiant), BallSpeed * MathF.Sin(angleInRadiant) * -1); // in godot > 0 is down and not up, because of that make it < 0
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }


}
