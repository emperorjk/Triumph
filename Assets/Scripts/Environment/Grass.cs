using UnityEngine;
using System.Collections;

public class Grass : EnvironmentBase
{
    public Grass(EnvironmentGameObject game)
        : base(game)
    {

    }
    public override EnvironmentTypes type
    {
        get { return EnvironmentTypes.Grass; }
    }
}
