using UnityEngine;
using System.Collections;


public class TrainingZone : BuildingsBase{
    // dit is de capture point. Deze naam vond ik toepasselijker - joey
    
    public TrainingZone(BuildingGameObject game) 
        : base(game, 50, 20)
    {

    }

    public override BuildingTypes type
    {
        get { return BuildingTypes.TrainingZone; }
    }

    public override Texture productionOverlay
    {
        get { throw new System.NotImplementedException(); }
    }

    public override Sprite sprite
    {
        get
        {
            throw new System.NotImplementedException();
        }
        protected set
        {
            throw new System.NotImplementedException();
        }
    }
}
