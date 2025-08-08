using Godot;
using System;

public partial class PassedPipeArea : Area2D
{
    [Signal]
    public delegate void PlayerPassedPipeEventHandler();
    public override void _Ready()
    {
        this.BodyExited += OnBodyExited;
    }

    private void OnBodyExited(Node2D body)
    {
        GD.Print(body);
        EmitSignal(SignalName.PlayerPassedPipe); 
    }

}
