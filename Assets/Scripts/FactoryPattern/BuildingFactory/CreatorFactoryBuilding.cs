using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CreatorFactoryBuilding
{
    private static BuildingGameObject ConfigBuildingAndTile(Tile tile, GameObject obj)
    {
        BuildingGameObject building = ((GameObject)GameObject.Instantiate(obj)).GetComponent<BuildingGameObject>();
        tile.buildingGameObject = building;
        building.transform.position = tile.transform.position;
        building.transform.parent = tile.transform;
        building.tile = tile;
        return building;
    }

    public static BuildingGameObject CreateBuilding(Tile tile, PlayerIndex index, BuildingTypes type)
    {
        if(tile.HasBuilding())
        {
            //throw new ArgumentException("The given tile already has a building on it. Cannot spawn a building.", "tile");
        }
        GameObject obj = null;
        if (type == BuildingTypes.TrainingZone)
        {
            TrainingzoneFactory fac = new TrainingzoneFactory();
            obj = fac.CreateBuilding(index);
        }
        return ConfigBuildingAndTile(tile, obj);
    }
}