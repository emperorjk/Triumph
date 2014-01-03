using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CaptureBuildings
{
    private List<BuildingsBase> buildings;
    /// <summary>
    /// Decrease the capture per turn if the building no longer has a unit on it.
    /// </summary>
    private int decreaseCapturePointsBy = 2;

    public CaptureBuildings()
    {
        buildings = new List<BuildingsBase>();
    }

    /// <summary>
    /// Call this method whenever a unit has moved onto an enemy or neutral building.
    /// </summary>
    /// <param name="building"></param>
    public void AddBuildingToCaptureList(BuildingsBase building) { 
        buildings.Add(building);
        Debug.Log("Building added");
    }

    /// <summary>
    /// This method is called by the GameManager within the NextPlayer method. It will calculate all of the capturing stuff.
    /// </summary>
    public void CalculateCapturing()
    {
        List<BuildingsBase> buildingsToBeRemoved = new List<BuildingsBase>();

        foreach (BuildingsBase building in buildings)
        {
            // Get the unit which is standing on the building.
            UnitGameObject unitOnBuilding = building.buildingGameObject.tile.unitGameObject;

            if (unitOnBuilding != null)
            {
                int health = unitOnBuilding.unitGame.health;
                building.IncreaseCapturePointsBy(health);
                Debug.Log("currentcapture points: " + building.currentCapturePoints);
                Debug.Log("capturepoints needed: " + building.capturePoints);
                // TO-DO: update the textbox or similair to display progress.
                // TO-DO: change the sprites to the new owner color sprite. 
                if (building.HasCaptured())
                {
                    Debug.Log("Building has been captured");
                    buildingsToBeRemoved.Add(building);
                    building.resetCurrentCapturePoints();
                    GameManager.Instance.GetPlayer(building.buildingGameObject.index).RemoveBuilding(building);
                    GameManager.Instance.GetPlayer(unitOnBuilding.index).AddBuilding(building);
                    building.buildingGameObject.index = unitOnBuilding.index;
                }
            }
            else
            {
                Debug.Log("NO longer a unit. Decreasing capture points.");
                Debug.Log("Before current cp: " + building.currentCapturePoints);
                // there is no longer a unit on the tile / building. Slowly decrease the capture points each turn.
                building.DecreaseCapturePointsBy(decreaseCapturePointsBy);
                Debug.Log("After current cp: " + building.currentCapturePoints);
                // TO-DO: disable the textbox or similair if currentcapture points are 0 or smaller.
            }
        }
        foreach (BuildingsBase item in buildingsToBeRemoved) { buildings.Remove(item); }
    }
}
