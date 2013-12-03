using UnityEngine;
using System.Collections;

public class BarracksRange : BuildingsBase {

    public BarracksRange()
        : base(20, 20)
    {

    }
    public override BuildingTypes type
    {
        get { return BuildingTypes.BarracksRange; }
    }
}
