using Godot;
using System;

public partial class PassedPipeArea : Area2D
{
    [Signal]
    public delegate void PlayerPassedPipeEventHandler();
    public override void _Ready()
    {
        this.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        EmitSignal(SignalName.PlayerPassedPipe); 
    }

}
