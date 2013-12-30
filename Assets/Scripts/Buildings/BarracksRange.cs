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

    public override bool CanProduce
    {
        get { return true; }
    }
}
