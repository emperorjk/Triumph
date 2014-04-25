using Assets.Scripts.Main;
using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.FactoryPattern.BuildingFactory
{
    public abstract class IBuildingGameObject
    {
        public string DirToBuildingFolder
        {
            get { return FileLocations.prefabBuildings; }
        }

        public abstract GameObject CreateBuilding(PlayerIndex index);
    }
}