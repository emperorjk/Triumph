using UnityEngine;
using System.Collections;

public class Forrest : EnvironmentBase
{
    public Forrest(EnvironmentGameObject game)
        : base(game)
    {

    }
    public override EnvironmentTypes type
    {
        get { return EnvironmentTypes.Forrest; }
    }
}
