using Godot;
using System;

public partial class Brick : StaticBody2D
{
    [Export] private Sprite2D texture { get; set; }

    public override void _Ready()
    {
        if (texture == null)
        {
            GD.PrintErr("Sprite2D not set for brick scene. Cannot set width and height.");
            return;
        }
    }

    public int Width()
    {
        return texture?.Texture.GetWidth() ?? 0;
    }

    public int Height()
    {
        return texture?.Texture.GetHeight() ?? 0;
    }

}
