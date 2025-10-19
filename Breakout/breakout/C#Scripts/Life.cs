using Godot;

public partial class Life : RichTextLabel
{
    public override void _Ready()
    {
        if (GetParent() is Ui ui && ui.LevelManager is not null)
        {
            ui.LevelManager.PlayerLifeChanged += UpdateLifePoints;
            UpdateLifePoints(ui.LevelManager.PlayerLife);
        }
    }

    private void UpdateLifePoints(int life)
    {
        this.Text = $"Life: {life}";
    }
}
