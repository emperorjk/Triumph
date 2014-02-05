using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CaptureBuildings : MonoBehaviour
{
    public List<Building> BuildingsBeingCaptured { get; private set; }
    private List<Building> buildingsToBeRemoved;
    /// <summary>
    /// Decrease the capture per turn if the building no longer has a unit on it. This can be placed in the UnitBase if certain buildings decrease faster than other buildings.
    /// </summary>
    private int decreaseCapturePointsBy = 2;

    void Awake ()
    {
        BuildingsBeingCaptured = new List<Building>();
        buildingsToBeRemoved = new List<Building>();
    }

    void OnDestroy()
    {
        buildingsToBeRemoved.Clear();
        buildingsToBeRemoved.Clear();
    }

    /// <summary>
    /// Call this method whenever a unit has moved onto an enemy or neutral building.
    /// </summary>
    /// <param name="building"></param>
    public void AddBuildingToCaptureList(Building building) {
        UnitGameObject unitOnBuilding = building.buildingGameObject.Tile.unitGameObject;
        if (!BuildingsBeingCaptured.Contains(building) && unitOnBuilding.index != building.buildingGameObject.index) 
        {
            BuildingsBeingCaptured.Add(building);
        }
        else if (BuildingsBeingCaptured.Contains(building) && unitOnBuilding.index == building.buildingGameObject.index)
        {
            building.resetCurrentCapturePoints();
            BuildingsBeingCaptured.Remove(building);
        }
    }

    /// <summary>
    /// This method is called by the GameManager within the NextPlayer method. It will calculate all of the capturing stuff.
    /// </summary>
    public void CalculateCapturing()
    {
        for (int i = 0; i < BuildingsBeingCaptured.Count; i++)
        {
            Building building = BuildingsBeingCaptured[i];

            UnitGameObject unitOnBuilding = building.buildingGameObject.Tile.unitGameObject;

            if (unitOnBuilding != null)
            {
                int health = unitOnBuilding.UnitGame.CurrentHealth;
                building.IncreaseCapturePointsBy(health);

                unitOnBuilding.UnitGame.DecreaseHealth((int)building.DamageToCapturingUnit);

                if (!unitOnBuilding.UnitGame.IsAlive())
                {
                    unitOnBuilding.UnitGame.OnDeath();
                }

                if (building.HasCaptured())
                {
                    if (building.buildingGameObject.type != BuildingTypes.Headquarters)
                    {
                        buildingsToBeRemoved.Add(building);
                        building.buildingGameObject.DestroyBuilding();
                        BuildingGameObject newBuilding = CreatorFactoryBuilding.CreateBuilding(unitOnBuilding.Tile, unitOnBuilding.index, building.buildingGameObject.type);
                        if (newBuilding.type == BuildingTypes.TrainingZone)
                        {
                            OnTrainingzoneCapturedHero(unitOnBuilding);
                        }
                    }
                    else
                    {
                        // Captured the HQ (Dissable level and _Scripts, show background color in winning player color and display winning text)
                        GameObject.FindGameObjectWithTag("Level").SetActive(false);
                        GameObject.Find("_Scripts").SetActive(false);
                        Camera.main.backgroundColor = GameManager.Instance.Players[unitOnBuilding.index].PlayerColor;
                        GameObject.Find("NotificationText").GetComponent<TextMesh>().text = unitOnBuilding.index.ToString() + " has won the game! \n\nPress anywhere to return to the menu.";
                        GameObject.Instantiate(Resources.Load<GameObject>(FileLocations.endGameLocation));
                    }
                }
            }
            else
            {
                // there is no longer a unit on the tile / building. Slowly decrease the capture points each turn.
                building.DecreaseCapturePointsBy(decreaseCapturePointsBy);
                if (building.currentCapturePoints <= 0)
                {
                    buildingsToBeRemoved.Add(building);
                }
            }
        }
        foreach (Building item in buildingsToBeRemoved) 
        {
            BuildingsBeingCaptured.Remove(item); 
        }
        buildingsToBeRemoved.Clear();
    }


    /// <summary>
    /// When a unit captured an trainingzone, train him to his hero form.
    /// </summary>
    /// <param name="unitToHero">The unit to train to an hero.</param>
    private void OnTrainingzoneCapturedHero(UnitGameObject unitToHero)
    {
        if(!unitToHero.isHero)
        {
            Tile tiletoSpawn = unitToHero.Tile;
            PlayerIndex index = unitToHero.index;
            UnitTypes type = unitToHero.type;

            // TODO: either here in code or in the prefab, depending on how we want to implement certain conversions apply the buffs to the hero.
            // The damage and range can go in prefab. But how about health? Does the hero gain full health or depending on the normal units health.
            unitToHero.DestroyUnit();
            //UnitGameObject hero = CreatorFactoryUnit.CreateHeroUnit(tiletoSpawn, index, type);
            CreatorFactoryUnit.CreateHeroUnit(tiletoSpawn, index, type);
        }
    }
}