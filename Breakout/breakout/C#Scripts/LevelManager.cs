using Godot;

public partial class LevelManager : Node
{
    [Export] public Ball Ball { get; set; }
    [Export] public int PlayerLife { get; set; } = 3;
    [Signal] public delegate void PlayerScoreIncreasedEventHandler(int score);
    [Signal] public delegate void PlayerLifeChangedEventHandler(int life);
    public int PlayerScore { get; private set; }

    public override void _Ready()
    {
        if (Ball is null)
        {
            GD.PrintErr("LevelManager: Ball dependency is missing");
            return;
        }
        Ball.BrickRemoved += IncreasePlayerScore;
        Ball.OutOfScreen += DecreasePlayerLife;
    }

    private void IncreasePlayerScore()
    {
        PlayerScore++;
        EmitSignal(SignalName.PlayerScoreIncreased, PlayerScore);
    }

    private void DecreasePlayerLife()
    {
        PlayerLife--;
        EmitSignal(SignalName.PlayerLifeChanged, PlayerLife);
    }
}
