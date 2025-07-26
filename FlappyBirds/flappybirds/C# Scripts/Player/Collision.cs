using Godot;

public partial class Collision : Area2D
{
    [Signal]
    public delegate void PlayerCollidedEventHandler();

    private Player _player;
    public void Init(Player player)
    {
        this._player = player;
    }

    public override void _Ready()
    {
        this.AreaEntered += OnAreaEntered;
        this.BodyEntered += OnBodyEntered;
    }


    // TODO do i need this? Depends if i can use the collition of objects without area
    private void OnAreaEntered(Area2D area)
    {
    }

    private void OnBodyEntered(Node2D body)
    {
        // player node has collision on top of area
        if (body is not Player)
            PlayerDies();
    }

    private void PlayerDies()
    {
        _player.QueueFree(); 
        EmitSignal(SignalName.PlayerCollided);
    }

}
