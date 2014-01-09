using UnityEngine;
using System.Collections;

public class Headquarters : BuildingsBase{

    public Headquarters(BuildingGameObject game)
        : base(game, 50, 30)
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
        get { return 0; }
    }
}
