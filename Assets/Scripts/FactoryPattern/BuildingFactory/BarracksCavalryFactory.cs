using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.FactoryPattern.BuildingFactory
{
    public class BarracksCavalryFactory : IBuildingGameObject
    {
        public override GameObject CreateBuilding(PlayerIndex index)
        {
            GameObject obj = null;
            if (PlayerIndex.Blue == index)
            {
                obj = Resources.Load<GameObject>(DirToBuildingFolder + "BarracksCavalryBluePrefab");
            }
            else if (PlayerIndex.Red == index)
            {
                obj = Resources.Load<GameObject>(DirToBuildingFolder + "BarracksCavalryRedPrefab");
            }
            else if (PlayerIndex.Neutral == index)
            {
                obj = Resources.Load<GameObject>(DirToBuildingFolder + "BarracksCavalryNeutralPrefab");
            }
            return obj;
        }
    }
}
