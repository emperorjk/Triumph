using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public class DayStateController : MonoBehaviour
{
    public DayStates CurrentDayState { get; private set; }

    /// <summary>
    /// Returns true is the fow is shown, e.g. it is active. False otherwise.
    /// </summary>
    public bool isFowActive { get; private set; }
    private int DayTurnCounter = 1;

    private GameManager _manager;

    public DayStateController()
    {
        _manager = GameManager.Instance;
        isFowActive = false;
        CurrentDayState = DayStates.Morning;
    }

    public void TurnIncrease()
    {
        DayTurnCounter++;
        bool ended = DayTurnCounter > _manager.LevelManager.CurrentLevel.dayNightTurns[CurrentDayState];
        if(ended)
        {
            Debug.Log("Previous daystate: " + CurrentDayState);
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
            Debug.Log("New daystate: " + CurrentDayState);
        }
    }

    /// <summary>
    /// Returns wether or not the current turn is a day or night time.
    /// </summary>
    /// <returns>Returns true if it is day time and false if it is night time.</returns>
    private bool GetIsDay()
    {
        return true;
        // Disabled because the day and night states have been implemented.
        // Will change this later.
        /*
        bool result = false;
        int number = _manager.CurrentTurn % 5;
        if (number >= 1 && number <= 3)
        {
            result = true;
        }
        else if (number == 0 || number == 4)
        {
            result = false;
        }

        return result;
         * */
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
        bool isDay = GetIsDay();

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

