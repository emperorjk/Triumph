using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Buildings;
using Assets.Scripts.Main;
using Assets.Scripts.Notification;
using Assets.Scripts.Players;
using Assets.Scripts.Tiles;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.DayNight
{
    public class DayStateController : MonoBehaviour
    {
        public DayStates CurrentDayState { get; private set; }

        private float speed = 0.5f;
        private float StartTime;
        private Light lightFront;
        private Light lightBack;

        private List<Fading> tiles;
        private float StartTimeFading;

        /// <summary>s
        /// Returns true is the fow is shown, e.g. it is active. False otherwise.
        /// </summary>
        public bool isFowActive { get; private set; }

        private bool lastIsFowActive;
        private int DayTurnCounter = 1;

        private GameManager _manager;

        private void Start()
        {
            lightFront = GameObject.Find("LightFront").GetComponent<Light>();
            lightBack = GameObject.Find("LightBack").GetComponent<Light>();

            _manager = GameObject.Find("_Scripts").GetComponent<GameManager>();
            isFowActive = lastIsFowActive = false;
            CurrentDayState = DayStates.Morning;
            // Start a fade into the CurrentDayState. This is done because the intensity of the light prefab may differ from the values set below.
            StartTime = Time.time;
            tiles = new List<Fading>();
        }

        private void Update()
        {
            if (StartTime > 0f)
            {
                float intensity = Mathf.Lerp(lightFront.intensity, GetIntensityLightForCurrentDayState(),
                    GetTimePassed(StartTime));
                lightFront.intensity = lightBack.intensity = intensity;
                if (GetTimePassed(StartTime) >= 1f)
                {
                    StartTime = 0f;
                }
            }

            if (StartTimeFading > 0f)
            {
                foreach (Fading item in tiles)
                {
                    Color color = item.t.FogOfWar.renderer.material.color;
                    color.a = Mathf.Lerp(item.from, item.to, GetTimePassed(StartTimeFading));
                    item.t.FogOfWar.renderer.material.color = color;

                    if (GetTimePassed(StartTimeFading) >= 1f)
                    {
                        StartTimeFading = 0f;
                        UpdateTextForAllTiles();
                    }
                }
                if (StartTimeFading <= 0f)
                {
                    tiles.Clear();
                }
            }
        }

        private void AddToFading(Tile t, float from, float to)
        {
            Fading f = new Fading();
            f.from = from;
            f.to = to;
            f.t = t;

            tiles.Add(f);
            StartTimeFading = Time.time;
        }

        public void TurnIncrease()
        {
            DayTurnCounter++;
            int turnsNeeded = _manager.LevelManager.CurrentLevel.dayNightTurns[CurrentDayState];
            bool ended = DayTurnCounter > turnsNeeded;

            if (ended)
            {
                int newNumber = (int) CurrentDayState + 1;
                int highest = 0;
                foreach (DayStates day in (DayStates[]) Enum.GetValues(typeof (DayStates)))
                {
                    int n = (int) day;

                    if (n > highest)
                    {
                        highest = n;
                    }

                    if (newNumber > highest)
                    {
                        CurrentDayState = DayStates.Morning;
                        DayTurnCounter = 1;
                    }
                    else if (n == newNumber)
                    {
                        CurrentDayState = day;
                        DayTurnCounter = 1;
                    }
                }
                StartTime = Time.time;
                Notificator.Notify(CurrentDayState.ToString() + " has arrived", 1.5f);
            }
            else
            {
                int turnsRemaining = (turnsNeeded - DayTurnCounter) + 1;
                Notificator.Notify(turnsRemaining + " turns remaining before new daystate!", 1.1f);
            }

            TurnFOW();
        }

        /// <summary>
        /// Gets the intensity level for the light. Change these values to make the world lighter or darker. 
        /// This is not the best piece of code yet creating a dictionary or something is waste of memory.
        /// </summary>
        /// <returns>The intensity level for the CurrentDayState</returns>
        private float GetIntensityLightForCurrentDayState()
        {
            switch (CurrentDayState)
            {
                case DayStates.Morning:
                    return 3.8f;
                case DayStates.Midday:
                    return 5f;
                case DayStates.Evening:
                    return 2.7f;
                case DayStates.Night:
                    return 1.4f;
                default:
                    return 5f;
            }
        }

        private float GetTimePassed(float t)
        {
            return (Time.time - t)/speed;
        }

        private void TurnFOW()
        {
            isFowActive = CurrentDayState == DayStates.Night;
            // If the last turn the fog was not active and it has now switched to active then loop through all tiles, 
            // if a tile does not exist in the view range of any friendly los than fade it dark. Ignore the rest.
            if (!lastIsFowActive && isFowActive)
            {
                List<Tile> allTiles = TileHelper.GetAllTilesInListType();
                List<Tile> tileInLOSRange = TileHelper.GetAllTilesWithPlayerLOS(_manager.CurrentPlayer.index);

                foreach (Tile tile in allTiles)
                {
                    if (!tileInLOSRange.Contains(tile))
                    {
                        tile.IsFogShown = true;
                        AddToFading(tile, 0f, 1f);
                    }
                }
            }
                // If the last and current turn are active fog then check if the fog is already shown, ifso then ignore else fade to dark.
                // Then 
            else if (lastIsFowActive && isFowActive)
            {
                foreach (Tile tile in TileHelper.GetAllTilesInListType())
                {
                    if (!tile.IsFogShown)
                    {
                        tile.IsFogShown = true;
                        AddToFading(tile, 0f, 1f);
                    }
                }
                foreach (Tile tile in TileHelper.GetAllTilesWithPlayerLOS(_manager.CurrentPlayer.index))
                {
                    tile.IsFogShown = false;
                    AddToFading(tile, 1f, 0f);
                }
            }
                // If the last turn was active and the current is not. Then fade all of the fog to light if it was previously active.
            else if (lastIsFowActive && !isFowActive)
            {
                foreach (Tile tile in TileHelper.GetAllTilesInListType())
                {
                    if (tile.IsFogShown)
                    {
                        tile.IsFogShown = false;
                        AddToFading(tile, 1f, 0f);
                    }
                }
            }

            lastIsFowActive = isFowActive;
        }

        /// <summary>
        /// Check if all of the unit or building textboxes in the game need to be active or not.
        /// </summary>
        private void UpdateTextForAllTiles()
        {
            foreach (Tile tile in TileHelper.GetAllTilesInListType())
            {
                if (tile.HasUnit())
                {
                    tile.unitGameObject.UpdateHealthText();
                }
                if (tile.HasBuilding())
                {
                    tile.buildingGameObject.UpdateCapturePointsText();
                }
            }
        }

        #region FOG method for particular user.

        public void HideFowWithinLineOfSight(PlayerIndex index)
        {
            CheckLineForAllObjects(index, false);
        }

        public void ShowFowWithinLineOfSight(PlayerIndex index)
        {
            CheckLineForAllObjects(index, true);
        }

        /// <summary>
        /// This methods updates all of the fog of war for the given player. Loops through all of the buildings and units in order to update the fog of war.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="showFog"></param>
        private void CheckLineForAllObjects(PlayerIndex index, bool showFog)
        {
            if (isFowActive)
            {
                Player player = _manager.Players[index];

                foreach (Unit item in player.ownedUnits)
                {
                    ShowOrHideFowWithinRange(item.UnitGameObject.Tile, item.FowLineOfSightRange, showFog);
                }

                foreach (Building item in player.ownedBuildings)
                {
                    ShowOrHideFowWithinRange(item.buildingGameObject.Tile, item.FowLineOfSightRange, showFog);
                }
            }

            // Update all of the health / capturepoints from all of the other players.
            foreach (
                Player players in
                    _manager.Players.Where(x => x.Value.index != index && x.Value.index != PlayerIndex.Neutral)
                        .Select(x => x.Value))
            {
                foreach (Unit unit in players.ownedUnits)
                {
                    unit.UnitGameObject.UpdateHealthText();
                }
            }
            foreach (Building building in _manager.CaptureBuildings.BuildingsBeingCaptured)
            {
                building.buildingGameObject.UpdateCapturePointsText();
            }

        }

        /// <summary>
        /// Hides the fow within a certain range of a given tile.
        /// </summary>
        /// <param name="tile">The tile from which to hide the fow.</param>
        /// <param name="rangeLineOfSight">The range from which to hide to fow.</param>
        /// <param name="showOrhide">Wether or not the fow should be enabled or disabled.</param>
        private void ShowOrHideFowWithinRange(Tile tile, int rangeLineOfSight, bool showOrhide)
        {
            tile.IsFogShown = showOrhide;

            // Fading when moving gives weird effects. For now disabled.
            //float fromf = showOrhide ? 0f : 1f;
            //float tof = showOrhide ? 1f : 0f;
            //AddToFading(tile, fromf, tof);
            Color cc = tile.FogOfWar.renderer.material.color;
            cc.a = tile.IsFogShown ? 1f : 0f;
            tile.FogOfWar.renderer.material.color = cc;

            foreach (
                KeyValuePair<int, Dictionary<int, Tile>> item in
                    TileHelper.GetAllTilesWithinRange(tile.Coordinate, rangeLineOfSight))
            {
                foreach (KeyValuePair<int, Tile> tileValue in item.Value)
                {
                    tileValue.Value.IsFogShown = showOrhide;

                    // Fading when moving gives weird effects. For now disabled.
                    //float from = showOrhide ? 0f : 1f;
                    //float to = showOrhide ? 1f : 0f;
                    //AddToFading(tileValue.Value, from, to);
                    Color color = tileValue.Value.FogOfWar.renderer.material.color;
                    color.a = tileValue.Value.IsFogShown ? 1f : 0f;
                    tileValue.Value.FogOfWar.renderer.material.color = color;
                }
            }
        }

        #endregion
    }
}