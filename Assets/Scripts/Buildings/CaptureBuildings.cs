using System.Collections.Generic;
using Assets.Scripts.FactoryPattern.BuildingFactory;
using Assets.Scripts.FactoryPattern.UnitFactory;
using Assets.Scripts.Main;
using Assets.Scripts.Players;
using Assets.Scripts.Tiles;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public class CaptureBuildings : MonoBehaviour
    {
        public List<Building> BuildingsBeingCaptured { get; private set; }

        private void Awake()
        {
            BuildingsBeingCaptured = new List<Building>();
        }

        private void OnDestroy()
        {
            BuildingsBeingCaptured.Clear();
        }

        /// <summary>
        /// Call this method whenever a unit has moved onto an enemy or neutral building.
        /// </summary>
        /// <param Name="building"></param>
        public void AddBuildingToCaptureList(Building building)
        {
            UnitGameObject unitOnBuilding = building.BuildingGameObject.Tile.unitGameObject;
            if (!BuildingsBeingCaptured.Contains(building) && unitOnBuilding.index != building.BuildingGameObject.index)
            {
                BuildingsBeingCaptured.Add(building);
            }
            else if (BuildingsBeingCaptured.Contains(building) &&
                     unitOnBuilding.index == building.BuildingGameObject.index)
            {
                building.ResetCurrentCapturePoints();
                BuildingsBeingCaptured.Remove(building);
            }
        }

        /// <summary>
        /// This method is called by the GameManager within the NextPlayer method. It will calculate all of the capturing stuff.
        /// </summary>
        public void CalculateCapturing()
        {
            int count = BuildingsBeingCaptured.Count;
            for (int i = 0; i < count; i++)
            {
                Building building = BuildingsBeingCaptured[i];
                if (building.BuildingGameObject.Tile.HasUnit())
                {
                    UnitGameObject unitOnBuilding = building.BuildingGameObject.Tile.unitGameObject;
                    float health = unitOnBuilding.UnitGame.CurrentHealth;
                    building.IncreaseCapturePointsBy(health);

                    unitOnBuilding.UnitGame.DecreaseHealth(building.DamageToCapturingUnit);

                    if (!unitOnBuilding.UnitGame.IsAlive())
                    {
                        unitOnBuilding.UnitGame.OnDeath();
                    }

                    if (building.HasCaptured())
                    {
                        if (building.BuildingGameObject.type != BuildingTypes.Headquarters)
                        {
                            BuildingTypes type = building.BuildingGameObject.type;

                            BuildingsBeingCaptured.Remove(building);
                            count--;
                            i--;
                            building.BuildingGameObject.DestroyBuilding();

                            if (type == BuildingTypes.TrainingZone)
                            {
                                OnTrainingzoneCapturedHero(unitOnBuilding);
                            }
                        }
                        else
                        {
                            // Because the _scripts and stuff gets set to inactive the static TileHelper methods can no longer access GameManager. This caused null references.
                            // Instead loaded the main menu. A new scene called AfterMath of ResultScreen or something must be created.
                            // If the game was won then the rest of the end turn calculating would also continue in the background, including fading and such. So loading a new scene 
                            // makes sure that no unnecessary code is being run.
                           
                            Camera.main.backgroundColor = GameObject.Find("_Scripts").GetComponent<GameManager>().Players[unitOnBuilding.index].PlayerColor;
                            GameObject.FindGameObjectWithTag("Level").SetActive(false);
                            GameObject.Find("_Scripts").SetActive(false);
                            GameObject.Find("NotificationText").GetComponent<TextMesh>().text = unitOnBuilding.index + " has won the game! \n\nPress anywhere to return to the menu.";
                            Instantiate(Resources.Load<GameObject>(FileLocations.endGameLocation));
                        }
                    }
                }
                else
                {
                    building.DecreaseCapturePointsBy(building.CapturePointsDecreasedBy);
                    if (building.CurrentCapturePoints <= 0f)
                    {
                        BuildingsBeingCaptured.Remove(building);
                        count--;
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// When a unit captured an trainingzone, train him to his hero form.
        /// </summary>
        /// <param Name="unitToHero">The unit to train to an hero.</param>
        private void OnTrainingzoneCapturedHero(UnitGameObject unitToHero)
        {
            if (!unitToHero.isHero)
            {
                Tile tiletoSpawn = unitToHero.Tile;
                PlayerIndex index = unitToHero.index;
                UnitTypes type = unitToHero.type;
                unitToHero.DestroyUnit();
                CreatorFactoryUnit.CreateHeroUnit(tiletoSpawn, index, type);
            }
        }
    }
}