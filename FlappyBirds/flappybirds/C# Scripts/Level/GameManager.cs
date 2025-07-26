using System;
using Godot;

public partial class GameManager : Node
{
    // if chunk is original size, there will be a gap if chunk is reused
    private const float _chunkLength = 90f;
    // Is moved to the left and has objects as a child
    [Export] private Node2D _scene;
    [Export] private PackedScene[] _groups;
    private float _levelSpeed = 75f;

    // starting point of first chunk. needed for init
    private const int _chunkLeftOffset = -2;
    // will be used to determine where a chunk has to be moved to right
    private const int _chunkRightOffset = 3;

    private float currenProgress = 0f;

    public override void _Ready()
    {
        // starting chunks
        for (int i = _chunkLeftOffset; i < _chunkRightOffset; i++)
            InstantiateChunk(i, 0);

        // first obstacle chunk at right offset
        InstantiateChunk(_chunkRightOffset);
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
        if (currenProgress >= _chunkLength)
        {
            //SetOutOfScreenNodeAtEnding();
            _scene.GetChild(0).QueueFree();
            InstantiateChunk(_chunkRightOffset);
            currenProgress = 0f;
        }
    }

    /// <summary>
    /// Chunks at first first place will be moved to end of chunks if level progression has advanced one chunk length
    /// </summary>
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
            ? GD.RandRange(1, _groups.Length - 1) // first chunk is only for beginning of the level
            : type].Instantiate<Node2D>();
        _scene.AddChild(group);
        group.GlobalPosition = CalculateChunkPosition(offset);
    }

    private Vector2 CalculateChunkPosition(int offset)
    {
       return Vector2.Right * offset * _chunkLength; 
    }



}
