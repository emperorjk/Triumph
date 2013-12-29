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
