using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class SwordsmanFactory : IUnitGameObject
{
    public override GameObject CreateUnit(PlayerIndex index)
    {
        GameObject obj = null;
        if (PlayerIndex.Blue == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "SwordsmanBluePrefab");
        }
        else if (PlayerIndex.Red == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "SwordsmanRedPrefab");
        }
        return obj;
    }

    public override GameObject CreateHeroUnit(PlayerIndex index)
    {
        GameObject obj = null;
        if (PlayerIndex.Blue == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "SwordsmanBlueHeroPrefab");
        }
        else if (PlayerIndex.Red == index)
        {
            obj = Resources.Load<GameObject>(DirToUnitFolder + "SwordsmanBlueHeroPrefab");
        }
        return obj;
    }
}
