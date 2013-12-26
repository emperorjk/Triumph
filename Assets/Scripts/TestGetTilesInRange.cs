using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TestGetTilesInRange
{
    private GameManager _manager;

    public TestGetTilesInRange()
    {
        _manager = GameManager.Instance;
    }

    /// <summary>
    /// This method returns all the tiles that are within a certain range calculated from the Tile which you specify.
    /// It takes into account out of bounds and doesn't add the tile the which has the same coordinates as the given center point. 
    /// The return type is the same as the tile collection in the GameManager for consistancy.
    /// The movement and attack classes / implementations should be responsible for further calculations.
    /// So if you want to highlight possible attack locations the attack class / method should loop through all tiles in range
    /// and only hightlight the tiles on which an enemy unit is placed.
    /// </summary>
    /// <param name="centerPointTileCoordinate">The tilecoordinate from which the calculation is done. </param>
    /// <param name="range">The range from which tiles get returned</param>
    public Dictionary<int, Dictionary<int, Tile>> GetAllTilesWithinRange(TileCoordinates centerPointTileCoordinate, int range)
    {
        // Check if the range is 0 or smaller.
        if (range <= 0)
        {
            throw new ArgumentOutOfRangeException("range", "The entered range is 0 or smaller. Please use a correct range");
        }

        if (!_manager.tiles.ContainsKey(centerPointTileCoordinate.ColumnId) || !_manager.tiles[centerPointTileCoordinate.ColumnId].ContainsKey(centerPointTileCoordinate.RowId))
        {
            throw new ArgumentOutOfRangeException("centerPointTileCoordinate", "The given center tile does not exist. Please give a valid TileCoordinate");
        }

        // collection for holding the possible tiles that are within range.
        Dictionary<int, Dictionary<int, Tile>> possibleLocations = new Dictionary<int, Dictionary<int, Tile>>();

        int columnId = centerPointTileCoordinate.ColumnId;
        int rowId = centerPointTileCoordinate.RowId;

        // The row size in which it goes up and down.
        int size = 0;

        int beginColumnId = columnId - range;
        int endColumnId = columnId + range;
        int currentColumnId = beginColumnId;

        while (currentColumnId <= endColumnId)
        {
            // If the current tilecoordinate falls outside the level dont bother getting it.
            if (!_manager.tiles.ContainsKey(currentColumnId))
            {
                currentColumnId++;
                size++;
                continue;
            }

            int beginRowId = rowId - size;
            int endRowId = rowId + size;
            int currentRowid = beginRowId;

            while (currentRowid <= endRowId)
            {
                // If the current tilecoordinate falls outside the level dont bother getting it.
                // And if the current tilecoordinate is on the same place as the original coordinate dont get it.
                if (!_manager.tiles[currentColumnId].ContainsKey(currentRowid) ||
                    (currentColumnId == centerPointTileCoordinate.ColumnId &&
                    currentRowid == centerPointTileCoordinate.RowId))
                {
                    currentRowid++;
                    continue;
                }
                // Get the tile from the tile list and add it to the return list.
                Tile t = _manager.GetTile(new TileCoordinates(currentColumnId, currentRowid));
                if (!possibleLocations.ContainsKey(currentColumnId))
                {
                    possibleLocations.Add(currentColumnId, new Dictionary<int, Tile>());
                }
                possibleLocations[currentColumnId].Add(currentRowid, t);
                currentRowid++;
            }
            currentColumnId++;
            // Determine if the currentColumnId has reached the center tile columnid, ifso start making the size smaller.
            size = currentColumnId <= columnId ? size += 1 : size -= 1;
        }
        return possibleLocations;
    }
}