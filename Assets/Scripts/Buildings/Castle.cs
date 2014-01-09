using UnityEngine;
using System.Collections;

public class Castle : BuildingsBase{

    public Castle(BuildingGameObject game)
        : base(game, 50, 40)
    {

    }
    public override BuildingTypes type
    {
        get { return buildingGameObject.type; }
    }

    public override bool CanProduce
    {
        get { return false; }
    }

    public override float DamageToCapturingUnit
    {
        get { return 1; }
    }

    public override int FowLineOfSightRange
    {
        get { return 1; }
    }
}
