using UnityEngine;
using System.Collections;

public class Road : EnvironmentBase
{
    public Road(EnvironmentGameObject game)
        : base(game)
    {

    }
    public override EnvironmentTypes type
    {
        get { return EnvironmentTypes.Road; }
    }
}
