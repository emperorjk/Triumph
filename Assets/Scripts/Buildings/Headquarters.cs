using UnityEngine;
using System.Collections;

public class Headquarters : BuildingsBase{

    public Headquarters(BuildingGameObject game)
        : base(game, 50, 30)
    {

    }
    public override BuildingTypes type
    {
        get { return BuildingTypes.Headquarters; }
    }

    public override Texture productionOverlay
    {
        get { throw new System.NotImplementedException(); }
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
}
