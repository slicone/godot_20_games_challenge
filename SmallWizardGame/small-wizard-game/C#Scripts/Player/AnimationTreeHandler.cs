using Godot;
using System;

/// <summary>
/// This node will have all animation logic for the player
/// </summary>
public partial class AnimationTreeHandler : Node
{
	[Export] public AnimationTree animationTree { get; set; }

    public override void _Ready()
    {
        if (animationTree is null)
			GD.PrintErr("Animation Tree Handler dependency missing in player");
    }


    public void SetAnimationTreeParameter(string parameter, string value)
    {
		animationTree?.Set(parameter, value);
    }

    /// <summary>
    /// If updated in States there will be a slight delay while changig state resulting in weird behavour in rotation
	/// Because of that update blend_position consistently in player
    /// </summary>
	public void UpdatePlayerBlendPosition(float direction)
    {
		if(direction != 0)
        {
			animationTree.Set("parameters/BasicMovement/run/blend_position", direction);
			animationTree.Set("parameters/BasicMovement/idle/blend_position", direction);
			animationTree.Set("parameters/BasicMovement/jump/blend_position", direction);
			animationTree.Set("parameters/BasicMovement/fall/blend_position", direction);
        }
    }
}
