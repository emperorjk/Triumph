using UnityEngine;
using System.Collections;

public class BarracksCavalry : BuildingsBase{

    public BarracksCavalry(BuildingGameObject game) :
        base(game, 20, 20)
    {

    }
    public override BuildingTypes type
    {
        get { return BuildingTypes.BarracksCavalry; }
    }

    public override bool CanProduce
    {
        get { return true; }
    }
}
