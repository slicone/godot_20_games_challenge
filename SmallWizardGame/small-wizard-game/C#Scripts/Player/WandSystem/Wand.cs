using Godot;

public partial class Wand : AbstractWand
{
    [Export] public Player player { get; set; }
    [Export] public float Cooldown { get; set; } = 0.3f;
    [Export] public Timer CooldownTimer { get; set; }
    private Spell currentSpell;
    private Spell[] spells;
    private PackedScene projectileScene = GD.Load<PackedScene>("res://Scenes/Player/WandSystem/projectile.tscn");

    // TODO signals for UI like change spell...
    public override void _Ready()
    {
        spells = SpellLoader.Instance.GetSpells();
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

        if (!CooldownTimer.IsStopped())
        {
            return;
        }
        CooldownTimer.Start(Cooldown);

        // x not needed, always shoot in positive direction (positive relative to player)
        var direction = Input.GetVector("aim-controller-left", "aim-controller-right", "aim-controller-top", "aim-controller-down");
        // if player is rotated by animation tree on going left, y vector will face opposite direction and thus input is reveresd too
        if (player.Rotation != 0)
        {
            direction *= -1;
        }
        // Take mouse direction if controller direction zero
        if (direction == Vector2.Zero)
        {
            direction = GetLocalMousePosition().Normalized();
        }
        // Player shouldn't be able to shoot behind wand
        if (direction.X < 0)
        {
            return;
        }
        SpawnProjectile(direction);
    }

    
    private void SpawnProjectile(Vector2 projectileDirection)
    {
        var projectile = projectileScene?.Instantiate<Projectile>();
        if (projectile is null)
        {
            GD.PrintErr("Wand can't initialize given projectile Scene, check path");
        }
        // projectile cannot be a child of player otherwise if player
        // rotates it will rotate the already spawned projectile
        // thus rotate projectile manually
        if (player.Rotation != 0)
        {
            projectileDirection *= -1;
        }
        projectile.Init(projectileDirection, CreateAttackFromCurrentSpell());
        projectile.Position = GlobalPosition;
        GetTree().CurrentScene.AddChild(projectile);
    }
}
