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

    public override Sprite sprite
    {
        get
        {
            throw new System.NotImplementedException();
        }
        protected set
        {
            throw new System.NotImplementedException();
        }
    }

    public override bool IsWalkable
    {
        get { return true; }
    }
}
