using Assets.Scripts.Buildings;
using Assets.Scripts.Players;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.FactoryPattern.BuildingFactory
{
    public class CreatorFactoryBuilding
    {
        private static BuildingGameObject ConfigBuildingAndTile(Tile tile, GameObject obj)
        {
            BuildingGameObject building = ((GameObject) GameObject.Instantiate(obj)).GetComponent<BuildingGameObject>();
            tile.buildingGameObject = building;
            building.transform.position = tile.transform.position;
            building.transform.parent = tile.transform;
            building.Tile = tile;
            return building;
        }

        public static BuildingGameObject CreateBuilding(Tile tile, PlayerIndex index, BuildingTypes type)
        {
            IBuildingGameObject bgo = null;
            GameObject obj = null;
            if (type == BuildingTypes.TrainingZone) { bgo = new TrainingzoneFactory(); }
            else if (type == BuildingTypes.BarracksCavalry) { bgo = new BarracksCavalryFactory(); }
            else if (type == BuildingTypes.BarracksMelee) { bgo = new BarracksMeleeFactory(); }
            else if (type == BuildingTypes.BarracksRange) { bgo = new BarracksRangeFactory(); }

            obj = bgo.CreateBuilding(index);
            return ConfigBuildingAndTile(tile, obj);
        }
    }
}