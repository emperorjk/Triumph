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
    public void AddBuildingToCaptureList(BuildingsBase building) { buildings.Add(building); }

    /// <summary>
    /// This method is called by the GameManager within the NextPlayer method. It will calculate all of the capturing stuff.
    /// </summary>
    public void CalculateCapturing()
    {
        foreach (BuildingsBase building in buildings)
        {
            // Get the unit which is standing on the building.
            UnitGameObject unitOnBuilding = building.buildingGameObject.tile.unitGameObject;

            if (unitOnBuilding != null)
            {
                int health = unitOnBuilding.unitGame.health;
                building.IncreaseCapturePointsBy(health);
                
                // TO-DO: update the textbox or similair to display progress.
                
                // If the building is captured then: 
                // - Remove the building from the owner list.
                // - Add the building to the new owner list of owned buildings.
                // - Change the index of the building to the new player index of the owned building.
                if (building.HasCaptured())
                {
                    buildings.Remove(building);
                    building.resetCurrentCapturePoints();
                    GameManager.Instance.GetPlayer(building.buildingGameObject.index).RemoveBuilding(building);
                    GameManager.Instance.GetPlayer(unitOnBuilding.index).AddBuilding(building);
                    building.buildingGameObject.index = unitOnBuilding.index;
                }
            }
            else
            {
                // there is no longer a unit on the tile / building. Slowly decrease the capture points each turn.
                building.DecreaseCapturePointsBy(decreaseCapturePointsBy);
                // TO-DO: disable the textbox or similair if currentcapture points are 0 or smaller.
            }
        }
    }
}
