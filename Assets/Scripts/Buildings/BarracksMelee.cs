using UnityEngine;
using System.Collections;

public class BarracksMelee : BuildingsBase {

    public BarracksMelee(BuildingGameObject game) :
        base(game, 20, 20)
    {

    }

    public override BuildingTypes type
    {
        get { return BuildingTypes.BarracksMelee; }
    }

    public override Texture productionOverlay
    {
        get { throw new System.NotImplementedException(); }
    }
}
