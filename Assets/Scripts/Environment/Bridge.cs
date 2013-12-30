using UnityEngine;
using System.Collections;

public class Bridge : EnvironmentBase
{
    public Bridge(EnvironmentGameObject game)
        : base(game)
    {

    }
    public override EnvironmentTypes type
    {
        get { return EnvironmentTypes.Bridge; }
    }

    public override bool IsWalkable
    {
        get { return true; }
    }
}
