using Godot;
using System;

public partial class BrickManager : Node
{
    [Export] public int ColumnLength { get; set; } = 12;
    [Export] public int RowLength { get; set; } = 5;
    [Export] public int BrickGap { get; set; } = 5;
    private PackedScene brickScene = ResourceLoader.Load<PackedScene>("res://Scenes/brick.tscn");

    public override void _Ready()
    {
        SpawnBricks();
    }


    private void SpawnBricks()
    {
        for (var i = 0; i < RowLength ; i++)
        {
            for (var j = 0; j < ColumnLength; j++)
            {
                var brick = brickScene.Instantiate<Brick>();
                var initPosX = (brick.Width() / 2) + BrickGap;
                var initPosY = (brick.Height() / 2) + BrickGap;
                var brickXPos =  initPosX + (brick.Width() * j) + BrickGap * j;
                var brickYPos =  initPosY + (brick.Height() * i) + BrickGap * i;  
                brick.GlobalPosition = new Vector2(brickXPos, brickYPos);
                AddChild(brick);
            }
        }
    }
}
