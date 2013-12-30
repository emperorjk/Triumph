using UnityEngine;
using System.Collections;

public class Water : EnvironmentBase
{
    public Water(EnvironmentGameObject game)
        : base(game)
    {

    }

    public override EnvironmentTypes type
    {
        get { return EnvironmentTypes.Water; }
    }

    public override bool IsWalkable
    {
        get { return false; }
    }
}
