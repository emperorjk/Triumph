using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.FactoryPattern.UnitFactory
{
    public class KnightFactory : IUnitGameObject
    {
        public override GameObject CreateUnit(PlayerIndex index)
        {
            GameObject obj = null;
            if (PlayerIndex.Blue == index)
            {
                obj = Resources.Load<GameObject>(DirToUnitFolder + "KnightBluePrefab");
            }
            else if (PlayerIndex.Red == index)
            {
                obj = Resources.Load<GameObject>(DirToUnitFolder + "KnightRedPrefab");
            }
            return obj;
        }

        public override GameObject CreateHeroUnit(PlayerIndex index)
        {
            GameObject obj = null;
            if (PlayerIndex.Blue == index)
            {
                obj = Resources.Load<GameObject>(DirToUnitFolder + "KnightBlueHeroPrefab");
            }
            else if (PlayerIndex.Red == index)
            {
                obj = Resources.Load<GameObject>(DirToUnitFolder + "KnightRedHeroPrefab");
            }
            return obj;
        }
    }
}