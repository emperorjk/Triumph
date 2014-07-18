using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.FactoryPattern.BuildingFactory
{
    public class BarracksMeleeFactory : IBuildingGameObject
    {
        public override GameObject CreateBuilding(PlayerIndex index)
        {
            GameObject obj = null;
            if (PlayerIndex.Blue == index)
            {
                obj = Resources.Load<GameObject>(DirToBuildingFolder + "BarracksMeleeBluePrefab");
            }
            else if (PlayerIndex.Red == index)
            {
                obj = Resources.Load<GameObject>(DirToBuildingFolder + "BarracksMeleeRedPrefab");
            }
            else if (PlayerIndex.Neutral == index)
            {
                obj = Resources.Load<GameObject>(DirToBuildingFolder + "BarracksMeleeNeutralPrefab");
            }
            return obj;
        }
    }
}
