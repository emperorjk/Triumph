using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class KnightFactory : IUnitGameObject
{
    public override GameObject CreateUnit(PlayerIndex index)
    {
        GameObject obj = null;
        if (PlayerIndex.One == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "KnightBluePrefab");
        }
        else if (PlayerIndex.Two == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "KnightRedPrefab");
        }
        return obj;
    }

    public override GameObject CreateHeroUnit(PlayerIndex index)
    {
        GameObject obj = null;
        if (PlayerIndex.One == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "KnightHeroBluePrefab");
        }
        else if (PlayerIndex.Two == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "KnightHeroRedPrefab");
        }
        return obj;
    }
}
