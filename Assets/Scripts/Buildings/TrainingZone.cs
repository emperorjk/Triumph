﻿using UnityEngine;
using System.Collections;


public class TrainingZone : BuildingsBase{

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

    public override float DamageToCapturingUnit
    {
        get { return 1; }
    }
}
