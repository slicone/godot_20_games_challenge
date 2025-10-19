using Godot;

public partial class Ui : Control
{
    [Export] public LevelManager LevelManager { get; set; }

    public override void _Ready()
    {
        if (LevelManager is null)
        {
            GD.PrintErr("GUI Score: LevelManager dependency is missing");
            return;
        }
    }


}
