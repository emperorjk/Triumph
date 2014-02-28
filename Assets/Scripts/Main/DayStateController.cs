using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public class DayStateController : MonoBehaviour
{
    public DayStates CurrentDayState { get; private set; }

    private float speed = 0.5f;
    private float StartTime;
    private Light lightFront;
    private Light lightBack;

    /// <summary>s
    /// Returns true is the fow is shown, e.g. it is active. False otherwise.
    /// </summary>
    public bool isFowActive { get; private set; }
    private int DayTurnCounter = 1;

    private GameManager _manager;

    void Start()
    {
        lightFront = GameObject.Find("LightFront").GetComponent<Light>();
        lightBack = GameObject.Find("LightBack").GetComponent<Light>();

        _manager = GameManager.Instance;
        isFowActive = false;
        CurrentDayState = DayStates.Morning;
        // Start a fade into the CurrentDayState. This is done because the intensity of the light prefab may differ from the values set below.
        StartTime = Time.time;
    }

    public void TurnIncrease()
    {
        DayTurnCounter++;
        int turnsNeeded = _manager.LevelManager.CurrentLevel.dayNightTurns[CurrentDayState];
        bool ended = DayTurnCounter > turnsNeeded;
        
        if(ended)
        {
            int newNumber = (int)CurrentDayState + 1;
            int highest = 0;
            foreach (DayStates day in (DayStates[])Enum.GetValues(typeof(DayStates)))
            {
                int n = (int)day;
                
                if(n > highest) { highest = n; }

                if (newNumber > highest)
                {
                    CurrentDayState = DayStates.Morning;
                    DayTurnCounter = 1;
                }
                else if(n == newNumber)
                {
                    CurrentDayState = day;              
                    DayTurnCounter = 1;
                }
            }
            StartTime = Time.time;
            Notificator.Notify(CurrentDayState.ToString() + " has arrived", 1.7f);
        }
    }

    /// <summary>
    /// Gets the intensity level for the light. Change these values to make the world lighter or darker. 
    /// This is not the best piece of code yet creating a dictionary or something is waste of memory.
    /// </summary>
    /// <returns>The intensity level for the CurrentDayState</returns>
    private float GetIntensityLightForCurrentDayState()
    {
        if (CurrentDayState == DayStates.Morning)
        {
            return 3.8f;
        }
        else if (CurrentDayState == DayStates.Midday)
        {
            return 5f;
        }
        else if (CurrentDayState == DayStates.Evening)
        {
            return 2.7f;
        }
        else if (CurrentDayState == DayStates.Night)
        {
            return 1.4f;
        }
        else
        {
            return 5f;
        }
    }

    private float GetTimePassed()
    {
        return (Time.time - StartTime) / speed;
    }

    void Update()
    {
        if(StartTime > 0f)
        {
            float intensity = Mathf.Lerp(lightFront.intensity, GetIntensityLightForCurrentDayState(), GetTimePassed());
            lightFront.intensity = lightBack.intensity = intensity;
            if(GetTimePassed() >= 1f)
            {
                StartTime = 0f;
            }
        }
    }

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
        Player player = _manager.Players[index];
        foreach (Unit item in player.ownedUnits)
        {
            ShowOrHideFowWithinRange(item.UnitGameObject.Tile, item.FowLineOfSightRange, showFog);
            item.UnitGameObject.UpdateHealthText();
        }

        foreach (Building item in player.ownedBuildings)
        {
            ShowOrHideFowWithinRange(item.buildingGameObject.Tile, item.FowLineOfSightRange, showFog);
        }

        UnitBuilingHealthCapturePoints(index);
    }

    private void UnitBuilingHealthCapturePoints(PlayerIndex index)
    {
        if(isFowActive)
        {
            foreach (Player player in _manager.Players.Where(x => x.Value.index != index && x.Value.index != PlayerIndex.Neutral).Select(x => x.Value))
            {
                foreach (Unit unit in player.ownedUnits)
                {
                    unit.UnitGameObject.UpdateHealthText();
                }
            }

            foreach (Building building in GameManager.Instance.CaptureBuildings.BuildingsBeingCaptured)
            {
                building.buildingGameObject.UpdateCapturePointsText();
            }

        }
    }

    /// <summary>
    /// This method is called inside the NextPlayer() method. If it is night time it shows all of the fow. Then it goes through all of the current player buildings and unit to enable there
    /// line of sight.
    /// </summary>
    public void ShowOrHideFowPlayer()
    {
        bool isDay = CurrentDayState != DayStates.Night;

        if (!isDay)
        {
            ShowOrHideFow(true);
            CheckLineForAllObjects(_manager.CurrentPlayer.index, false);
       }
        else if (isFowActive && isDay)
        {
            ShowOrHideFow(false);
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
        if (isFowActive)
        {
            tile.FogOfWar.renderer.enabled = showOrhide;

            Dictionary<int, Dictionary<int, Tile>> tilesInRange = TileHelper.GetAllTilesWithinRange(tile.Coordinate, rangeLineOfSight);

            foreach (KeyValuePair<int, Dictionary<int, Tile>> item in tilesInRange)
            {
                foreach (KeyValuePair<int, Tile> tileValue in item.Value)
                {
                    tileValue.Value.FogOfWar.renderer.enabled = showOrhide;
                }
            }
        }
    }

    /// <summary>
    /// Show or hide all of the fog of war gameobjects.
    /// </summary>
    /// <param name="showFow"></param>
    private void ShowOrHideFow(bool showFow)
    {
        isFowActive = showFow;
        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in _manager.Tiles)
        {
            foreach (KeyValuePair<int, Tile> tile in item.Value)
            {
                tile.Value.FogOfWar.renderer.enabled = showFow;
                if(tile.Value.HasUnit())
                {
                    tile.Value.unitGameObject.UpdateHealthText();
                }
                if(tile.Value.HasBuilding())
                {
                    tile.Value.buildingGameObject.UpdateCapturePointsText();
                }
            }
        }
    }
}

