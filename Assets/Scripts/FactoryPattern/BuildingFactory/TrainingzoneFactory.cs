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
        if (PlayerIndex.Blue == index)
        {
            obj = Resources.Load<GameObject>(DirToBuildingFolder + "TrainingZoneBluePrefab");
        }
        else if (PlayerIndex.Red == index)
        {
            obj = Resources.Load<GameObject>(DirToBuildingFolder + "TrainingZoneRedPrefab");
        }
        else if (PlayerIndex.Neutral == index)
        {
            obj = Resources.Load<GameObject>(DirToBuildingFolder + "TrainingZoneNeutralPrefab");
        }
        return obj;
    }
}
