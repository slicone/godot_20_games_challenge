using System;
using Godot;

public partial class GameManager : Node
{
    // if chunk is original size, there will be a gap if chunk is reused
    private const float _groupLength = 90f;
    // Is moved to the left and has objects as a child
    [Export] private Node2D _scene;
    [Export] private PackedScene[] _groups;
    private float _levelSpeed = 100f;

    // starting point of first chunk. needed for init
    private const int _chunkLeftOffset = -1;
    // will be used to determine where a chunk has to be moved to right
    private const int _chunkRightOffset = 3;

    private float currenProgress = 0f;

    public override void _Ready()
    {   
        // starting chunks
        for (int i = -2; i < 2; i++)
            InstantiateChunk(i, 0);
        
        // random chunks
        for (int i = _chunkLeftOffset; i <= _chunkRightOffset; i++)
            InstantiateChunk(i);
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveLevel((float)delta); 
    }

    private void MoveLevel(float delta)
    {
        var progress = _levelSpeed * delta;
        foreach (Node n in _scene.GetChildren())
            ((Node2D)n).Translate(Vector2.Left * progress);

        currenProgress += progress;
        if (currenProgress >= _groupLength)
        {
            SetOutOfScreenNodeAtEnding();
            currenProgress = 0f;
        }
    }

    private void SetOutOfScreenNodeAtEnding()
    {
        var offScreenNode = _scene.GetChild<Node2D>(0);
        _scene.RemoveChild(offScreenNode);
        offScreenNode.GlobalPosition = CalculateChunkPosition(_chunkRightOffset);
        _scene.AddChild(offScreenNode);
    }

    /// <summary>
    /// Instantiate scene as node in moving scene 
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="type">Which type of scene should be added to the scene node</param>
    private void InstantiateChunk(int offset = 3, int type = -1)
    {
        Node2D group = _groups[(type == -1)
            ? GD.RandRange(0, _groups.Length - 1)
            : type].Instantiate<Node2D>();
        _scene.AddChild(group);
        group.GlobalPosition = CalculateChunkPosition(offset);
    }

    private Vector2 CalculateChunkPosition(int offset)
    {
       return Vector2.Right * offset * _groupLength; 
    }



}
