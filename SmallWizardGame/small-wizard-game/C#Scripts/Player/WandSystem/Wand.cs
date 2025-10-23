using System.Collections.Generic;
using Godot;

public partial class Wand : AbstractWand
{
    [Export] private Player player { get; set; }
    private Spell currentSpell;
    private List<Spell> spells = []; // TODO maybe SpellManager? as Singleton instance for other entites to use it
    private PackedScene projectileScene = GD.Load<PackedScene>("res://Scenes/Player/WandSystem/projectile.tscn");

    // TODO signals for UI like change spell, attack ...
    public override void _Ready()
    {
        string[] spellNames = ["FireSpell", "IceSpell"]; // TODO maybe better solution? Maybe GlobalEnum? If enemies use spells too
        foreach (string spellName in spellNames)
        {
            var spell = ResourceLoader.Load<Spell>($"res://Resources/Spells/{spellName}.tres");
            if (spell is null)
            {
                GD.PrintErr($"Spell {spellName} not found as .tres resource");
                continue;
            }
            spells.Add(spell);
        }
    }

    private Attack CreateAttackFromCurrentSpell()
    {
        if (currentSpell == null)
        {
            return new Attack();
        }
        return new Attack()
        {
            AttackDamage = currentSpell.damage,
            AttackPosition = Position,
        };
    }

    /// <summary>
    /// Attack with wand.
    /// Takes mouse and controller input. Though controller is dominant over mouse.
    /// </summary>
    public override void Attack()
    {
        // TODO problem here is that if controller is used and no direction is given it will take the mouse, but that probalby isn't the intended behaviour
        // for controller player. Maybe it is rather needed to check if controller is actively used?
        // Maybe check if controller is plugged in att all and then make a class that checks for active input?

        // x not needed, always shoot in positive direction (positive relative to player)
        var direction = Input.GetVector("aim-controller-left", "aim-controller-right", "aim-controller-top", "aim-controller-down");
        // if player is rotated by animation tree on going left, y vector will face opposite direction and thus input is reveresd too
        if (player.Rotation != 0)
        {
            direction *= -1;
        }
        // Take mouse direction if controller direction zero
        if (direction.Y == 0)
        {
            direction = GetLocalMousePosition().Normalized();
        }
        // Player shouldn't be able to shoot behind wand
        if(direction.X < 0)
        {
            return;
        }
        var projectile = projectileScene?.Instantiate<Projectile>();
        if(projectile is null)
        {
            GD.PrintErr("Wand can't initialize given projectile Scene, check path");
        }
        projectile.Init(direction, CreateAttackFromCurrentSpell());
        AddChild(projectile);
    }
}
