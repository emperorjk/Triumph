using UnityEngine;
using System.Collections;


public class TrainingZone : BuildingsBase{
    // dit is de capture point. Deze naam vond ik toepasselijker - joey
    
    public TrainingZone(BuildingGameObject game) 
        : base(game, 50, 40)
    {

    }

    public override BuildingTypes type
    {
        get { return BuildingTypes.TrainingZone; }
    }

    public override bool CanProduce
    {
        get { return false; }
    }
}
