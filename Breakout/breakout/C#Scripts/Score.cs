using Godot;

public partial class Score : RichTextLabel
{
    public override void _Ready()
    {
        if (GetParent() is Ui ui && ui.LevelManager is not null)
        {
            ui.LevelManager.PlayerScoreIncreased += IncreaseLabelScore;
        }
    }

    private void IncreaseLabelScore(int score) {
        this.Text = $"Score: {score}";
    }

}
