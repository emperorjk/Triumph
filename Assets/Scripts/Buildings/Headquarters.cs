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

    public override bool CanProduce
    {
        get { return false; }
    }
}
