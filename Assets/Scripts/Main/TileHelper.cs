using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TileHelper
{
    private static GameManager _manager = GameManager.Instance;

    /// <summary>
    /// Add a tile to the list. This methods should only be called one when a Tile GameObject is loaded when the scene starts.
    /// </summary>
    /// <param name="tile"></param>
    public static void AddTile(Tile tile)
    {
        // Check if the second dictionary exists in the list. If not then create a new dictionary and insert this in the tiles dictionary.
        if (!_manager.Tiles.ContainsKey(tile.Coordinate.ColumnId))
        {
            _manager.Tiles.Add(tile.Coordinate.ColumnId, new Dictionary<int, Tile>());
        }
        // Last insert the tile object into the correct spot in the dictionarys. Since we now know that both dictionary at these keys exist.
        _manager.Tiles[tile.Coordinate.ColumnId].Add(tile.Coordinate.RowId, tile);
    }

    /// <summary>
    /// Removes all entrys from the tiles dictionary. Should be called before a new scene (level) starts to load. So no old references exists when new Tile are added to the list.
    /// </summary>
    public static void ClearTilesDictionary()
    {
        _manager.Tiles.Clear();
    }

    /// <summary>
    /// Returns the tile with via the given TileCoordinates from the tiles dictionary. Or an KeyNotFoundException if either of the keys is not found.
    /// </summary>
    /// <param name="coor"></param>
    /// <returns></returns>
    public static Tile GetTile(TileCoordinates coor)
    {
        if (_manager.Tiles.ContainsKey(coor.ColumnId) && _manager.Tiles[coor.ColumnId].ContainsKey(coor.RowId))
        {
            return _manager.Tiles[coor.ColumnId][coor.RowId];
        }

        return null;
    }
	
	/// <summary>
    /// Returns whether or not vector2 a is within the Triumph range of vector2 b. So the tile distance.
    /// </summary>
    /// <param name="a">The first position from which to calculate the distance from.</param>
    /// <param name="b">The second position from which to calculate the distance from.</param>
    /// <param name="range">The range from which you want to check.</param>
    /// <returns></returns>
    public static bool IsTileWithinRange(Vector2 a, Vector2 b, int range)
    {
        // NOTE!!!!!!!
        // Because are sprites are a certain size. The location of the tiles is always in increments of 2 or negative 2. So tile(1,1) is on 0,0,0. tile(2,1) is on 2,0,0. 
        // tile(1,2) is on 0,-2,0 etc.
        // Because of these increments, 2 unity meters equals to 1 range. When we say attackrange of 1 you can attack units directly beside you.
        // So thats why we devide the distance between the two units by 2 in order to get the correct distance.
        // If we later have different sprite sizes and/or different increments this needs to change.
        // This works for all ranges.

        float distance = Vector2.Distance(a, b);
        float tileDistance = distance / 2;

        return tileDistance <= range;
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
    public static Dictionary<int, Dictionary<int, Tile>> GetAllTilesWithinRange(TileCoordinates centerPointTileCoordinate, int range)
    {
        // Check if the range is 0 or smaller.
        if (range <= 0)
        {
            throw new ArgumentOutOfRangeException("range", "The entered range is 0 or smaller. Please use a correct range");
        }

        if (!_manager.Tiles.ContainsKey(centerPointTileCoordinate.ColumnId) || !_manager.Tiles[centerPointTileCoordinate.ColumnId].ContainsKey(centerPointTileCoordinate.RowId))
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
            if (!_manager.Tiles.ContainsKey(currentColumnId))
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
                if (!_manager.Tiles[currentColumnId].ContainsKey(currentRowid) ||
                    (currentColumnId == centerPointTileCoordinate.ColumnId &&
                    currentRowid == centerPointTileCoordinate.RowId))
                {
                    currentRowid++;
                    continue;
                }
                // Get the tile from the tile list and add it to the return list.
                Tile t = GetTile(new TileCoordinates(currentColumnId, currentRowid));
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