using UnityEngine;
using System.Collections;

public class Small_Rocks : EnvironmentBase
{
    public Small_Rocks(EnvironmentGameObject game)
        : base(game)
    {

    }
    public override EnvironmentTypes type
    {
        get { return EnvironmentTypes.Small_Rocks; }
    }

    public override bool IsWalkable
    {
        get { return true; }
    }
}
