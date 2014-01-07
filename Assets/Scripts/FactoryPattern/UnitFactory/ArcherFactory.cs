using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class ArcherFactory : IUnitGameObject
{
    public override GameObject CreateUnit(PlayerIndex index)
    {
        GameObject obj = null;
        if (PlayerIndex.Blue == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "ArcherBluePrefab");
        }
        else if (PlayerIndex.Red == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "ArcherRedPrefab");
        }
        return obj;
    }

    public override GameObject CreateHeroUnit(PlayerIndex index)
    {
        GameObject obj = null;
        if (PlayerIndex.Blue == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "ArcherBlueHeroPrefab");
        }
        else if (PlayerIndex.Red == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "ArcherRedHeroPrefab");
        }
        return obj;
    }
}
