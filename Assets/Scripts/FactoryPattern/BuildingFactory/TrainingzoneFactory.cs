using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class TrainingzoneFactory : IBuildingGameObject
{
    public override GameObject CreateBuilding(PlayerIndex index)
    {
        GameObject obj = null;
        if (PlayerIndex.One == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "TrainingZoneBluePrefab");
        }
        else if (PlayerIndex.Two == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "TrainingZoneRedPrefab");
        }
        else if (PlayerIndex.Neutral == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "TrainingZoneNeutralPrefab");
        }
        return obj;
    }
}
