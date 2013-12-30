using System;
using UnityEngine;
using System.Collections;

public class Dirt : EnvironmentBase
{
    public Dirt(EnvironmentGameObject game)
        : base(game)
    {

    }

    public override bool IsWalkable
    {
        get { return true; }
    }

    public override EnvironmentTypes type
    {
        get { return EnvironmentTypes.Dirt; }
    }
}
