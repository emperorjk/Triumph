using UnityEngine;
using System.Collections;

public class Castle : BuildingsBase{

    public Castle(BuildingGameObject game)
        : base(game, 50, 40)
    {

    }
    public override BuildingTypes type
    {
        get { return BuildingTypes.Castle; }
    }

    public override bool CanProduce
    {
        get { return false; }
    }
}
