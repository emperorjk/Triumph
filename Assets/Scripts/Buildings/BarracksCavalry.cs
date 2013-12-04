using UnityEngine;
using System.Collections;

public class BarracksCavalry : BuildingsBase{

    public BarracksCavalry() :
        base(20, 20)
    {

    }
    public override BuildingTypes type
    {
        get { return BuildingTypes.BarracksCavalry; }
    }

    public override Texture productionOverlay
    {
        get { throw new System.NotImplementedException(); }
    }
}
