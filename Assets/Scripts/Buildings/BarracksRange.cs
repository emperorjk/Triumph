using UnityEngine;
using System.Collections;

public class BarracksRange : BuildingsBase {

    public BarracksRange(BuildingGameObject game)
        : base(game, 20, 20)
    {

    }
    public override BuildingTypes type
    {
        get { return BuildingTypes.BarracksRange; }
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
