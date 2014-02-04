using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    /// <summary>
    /// Returns true is the fow is shown, e.g. it is active. False otherwise.
    /// </summary>
    public bool isFowActive { get; private set; }
    private GameManager _manager;

    public FogOfWar()
    {
        _manager = GameManager.Instance;
        isFowActive = false;
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
        foreach (UnitBase item in player.ownedUnits)
        {
            ShowOrHideFowWithinRange(item.UnitGameObject.Tile, item.FowLineOfSightRange, showFog);
        }

        foreach (BuildingsBase item in player.ownedBuildings)
        {
            ShowOrHideFowWithinRange(item.buildingGameObject.tile, item.FowLineOfSightRange, showFog);
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
    /// Returns wether or not the current turn is a day or night time.
    /// </summary>
    /// <returns>Returns true if it is day time and false if it is night time.</returns>
    private bool GetIsDay()
    {
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
            }
        }
    }
}

