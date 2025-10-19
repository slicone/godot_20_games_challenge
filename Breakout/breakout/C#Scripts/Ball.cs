using Godot;
using System;

public partial class Ball : CharacterBody2D
{
    [Export] public Area2D area2D { get; set; }
    [Export] public int BallSpeed { get; set; } = 100;
    [Export] public int BallSpeedIncreaseOnBrick { get; set; } = 10;
    [Signal] public delegate void BrickRemovedEventHandler();
    [Signal] public delegate void OutOfScreenEventHandler();

    public override void _Ready()
    {
        area2D.BodyEntered += BodyEntered;
        var angle = 65;
        var angleInRadiant = Mathf.DegToRad(angle);
        Velocity = new Vector2(BallSpeed * MathF.Cos(angleInRadiant), BallSpeed * MathF.Sin(angleInRadiant));

    }
    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    private void BodyEntered(Node2D body)
    {
        switch (body)
        {
            // will otherwise detect area collision
            case Ball:
                return;

            case Platform platform:
                CalculateNewBallAngle(platform);
                return;

            case VerticalWall:
                Velocity = Velocity.Reflect(Vector2.Down);
                break;

            case Ceiling:
                Velocity = Velocity.Reflect(Vector2.Right);
                break;

            case Bottom:
                QueueFree();
                EmitSignal(SignalName.OutOfScreen);
                // TODO respawn ball
                break;
            // TODO probably have to distinct if ball hits top/bottom or left/right of brick
            // Because it will bounce weird if hit from left/right side - like in a straight horizontal direction
            case Brick brick: 
                IncreaseBallSpeed();
                var velocityNorm = Velocity.Reflect(Vector2.Right).Normalized();
                Velocity = new Vector2(velocityNorm.X * BallSpeed, velocityNorm.Y * BallSpeed);
                brick.QueueFree();
                EmitSignal(SignalName.BrickRemoved);
                break;
        }

    }

    private void CalculateNewBallAngle(Platform platform)
    {
        var relativePlatformHit = (GlobalPosition.X - platform.GlobalPosition.X) / (platform.Sprite.Texture.GetWidth() / 2);
        if (relativePlatformHit < 0)
        {
            // shift range of relativePlatformHit values between 0 and 1
            // otherwise angle will be under the minimal reflective angle
            var newAngle = Mathf.Lerp(platform.MaxDirectionAngle, platform.MinDirectionAngle, relativePlatformHit * -1);
            var angleInRadiant = Mathf.DegToRad(newAngle);
            var velocityNorm = new Vector2(MathF.Cos(angleInRadiant) * -1, MathF.Sin(angleInRadiant) * -1).Normalized();
            Velocity = new Vector2(BallSpeed * velocityNorm.X, BallSpeed * velocityNorm.Y); // direct in negative x, because plattform hit left part of platform 
        }
        else
        {
            var newAngle = Mathf.Lerp(platform.MaxDirectionAngle, platform.MinDirectionAngle, relativePlatformHit);
            var angleInRadiant = Mathf.DegToRad(newAngle);
            var velocityNorm = new Vector2(MathF.Cos(angleInRadiant), MathF.Sin(angleInRadiant) * -1).Normalized();
            Velocity = new Vector2(BallSpeed * velocityNorm.X, BallSpeed * velocityNorm.Y); 
        }
    }

    private void IncreaseBallSpeed()
    {
        BallSpeed += BallSpeedIncreaseOnBrick; 
    }

}
