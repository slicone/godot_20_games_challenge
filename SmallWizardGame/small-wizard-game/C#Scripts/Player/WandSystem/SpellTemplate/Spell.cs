using Godot;
using System;

[GlobalClass] 
public partial class Spell : Resource
{
    [Export] public float damage;
    [Export] public Animation animation;
}
