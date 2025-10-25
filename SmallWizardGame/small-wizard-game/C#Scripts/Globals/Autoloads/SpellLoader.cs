using System;
using System.Collections.Generic;
using Godot;

public partial class SpellLoader : Node
{
    public static SpellLoader Instance { get; private set; }
    private List<Spell> spells = [];
    private Spell[] spellsArray = null;

    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.PrintErr("SpellLoader already exists!");
            QueueFree();
            return;
        }
        Instance = this;

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

    public Spell[] GetSpells()
    {
        if(spellsArray == null)
        {
            spellsArray = spells.ToArray();
        }
        return spellsArray;
    }
}
