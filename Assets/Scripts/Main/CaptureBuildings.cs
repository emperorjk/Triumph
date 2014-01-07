using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CaptureBuildings
{
    private List<BuildingsBase> buildings;
    private List<BuildingsBase> buildingsToBeRemoved;
    /// <summary>
    /// Decrease the capture per turn if the building no longer has a unit on it. This can be placed in the UnitBase if certain buildings decrease faster than other buildings.
    /// </summary>
    private int decreaseCapturePointsBy = 2;

    public CaptureBuildings()
    {
        buildings = new List<BuildingsBase>();
        buildingsToBeRemoved = new List<BuildingsBase>();
    }

    /// <summary>
    /// Call this method whenever a unit has moved onto an enemy or neutral building.
    /// </summary>
    /// <param name="building"></param>
    public void AddBuildingToCaptureList(BuildingsBase building) {
        UnitGameObject unitOnBuilding = building.buildingGameObject.tile.unitGameObject;
        if (!buildings.Contains(building) && unitOnBuilding.index != building.buildingGameObject.index) 
        {
            buildings.Add(building);
        }
        else if (buildings.Contains(building) && unitOnBuilding.index == building.buildingGameObject.index)
        {
            building.resetCurrentCapturePoints();
            //building.buildingGameObject.UpdateCapturePointsText();
            buildings.Remove(building);
        }
    }

    /// <summary>
    /// This method is called by the GameManager within the NextPlayer method. It will calculate all of the capturing stuff.
    /// </summary>
    public void CalculateCapturing()
    {
        foreach (BuildingsBase building in buildings)
        {
            UnitGameObject unitOnBuilding = building.buildingGameObject.tile.unitGameObject;

            if (unitOnBuilding != null)
            {
                int health = unitOnBuilding.unitGame.currentHealth;
                building.IncreaseCapturePointsBy(health);

                unitOnBuilding.unitGame.DecreaseHealth((int)building.DamageToCapturingUnit);

                if (building.HasCaptured())
                {
                    buildingsToBeRemoved.Add(building);
                    GameManager.Instance.GetPlayer(building.buildingGameObject.index).RemoveBuilding(building);
                    GameObject.Destroy(building.buildingGameObject.gameObject);
                    CreatorFactoryBuilding.CreateBuilding(unitOnBuilding.tile, unitOnBuilding.index, building.buildingGameObject.type);
                }
            }
            else
            {
                // there is no longer a unit on the tile / building. Slowly decrease the capture points each turn.
                building.DecreaseCapturePointsBy(decreaseCapturePointsBy);
                if(building.currentCapturePoints <= 0)
                {
                    buildingsToBeRemoved.Add(building);
                }
            }
        }
        foreach (BuildingsBase item in buildingsToBeRemoved) 
        {
            buildings.Remove(item); 
        }
        buildingsToBeRemoved.Clear();
    }
}
