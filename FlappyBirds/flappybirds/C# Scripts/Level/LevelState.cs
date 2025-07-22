using System;
using Godot;

public partial class LevelState : Node
{
    [Export]
    public Player Player { get; set; }

    [Signal]
    public delegate void ScoreIncrementedEventHandler();

    public int Score { get; private set; }

    public override void _Ready()
    {
        Player.Collision.PlayerCollided += PlayerDies;
    }


    public void IncrementScore()
    {
        Score++;
        EmitSignal(SignalName.ScoreIncremented);
    }

    private void PlayerDies()
    {
        GD.Print("Player died");
    }
}
