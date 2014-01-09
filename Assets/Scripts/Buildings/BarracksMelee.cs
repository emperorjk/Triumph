using UnityEngine;
using System.Collections;

public class BarracksMelee : BuildingsBase {

    public BarracksMelee(BuildingGameObject game) :
        base(game, 20, 20)
    {

    }

    public override BuildingTypes type
    {
        get { return buildingGameObject.type; }
    }

    public override bool CanProduce
    {
        get { return true; }
    }

    public override float DamageToCapturingUnit
    {
        get { return 0; }
    }

    public override int FowLineOfSightRange
    {
        get { return 1; }
    }
}
