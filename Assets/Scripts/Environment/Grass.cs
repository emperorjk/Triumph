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
