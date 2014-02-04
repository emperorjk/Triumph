using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class EnvironmentBase
{
    public EnvironmentBase(EnvironmentGameObject game, bool isWalkable, Dictionary<UnitTypes, float> modifiers)
    {
        this.environmentGameObject = game;
        this.IsWalkable = isWalkable;
        this.modifiers = modifiers;
    }
    public EnvironmentGameObject environmentGameObject { get; private set; }
    public bool IsWalkable { get; set; }
    private Dictionary<UnitTypes, float> modifiers { get; set; }
    public float GetModifier(UnitTypes type) { return modifiers[type]; }
}
