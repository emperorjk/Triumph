﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Buildings;
using Assets.Scripts.Main;
using Assets.Scripts.Players;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    public class TileHelper
    {
        /// <summary>
        /// Add a Tile to the list. This methods should only be called one when a Tile GameObject is loaded when the scene starts.
        /// </summary>
        /// <param Name="Tile"></param>
        public static void AddTile(Tile tile)
        {
            GameManager _manager = GameObject.Find("_Scripts").GetComponent<GameManager>();
            // Check if the second dictionary exists in the list. If not then create a new dictionary and insert this in the tiles dictionary.
            if (!_manager.Tiles.ContainsKey(tile.Coordinate.ColumnId))
            {
                _manager.Tiles.Add(tile.Coordinate.ColumnId, new Dictionary<int, Tile>());
            }
            // Last insert the Tile object into the correct spot in the dictionarys. Since we now know that both dictionary at these keys exist.
            _manager.Tiles[tile.Coordinate.ColumnId].Add(tile.Coordinate.RowId, tile);
        }

        /// <summary>
        /// Returns the Tile with via the given TileCoordinates from the tiles dictionary. Or an KeyNotFoundException if either of the keys is not found.
        /// </summary>
        /// <param Name="coor"></param>
        /// <returns></returns>
        public static Tile GetTile(TileCoordinates coor)
        {
            GameManager _manager = GameObject.Find("_Scripts").GetComponent<GameManager>();
            if (_manager.Tiles.ContainsKey(coor.ColumnId) && _manager.Tiles[coor.ColumnId].ContainsKey(coor.RowId))
            {
                return _manager.Tiles[coor.ColumnId][coor.RowId];
            }

            return null;
        }

        /// <summary>
        /// Returns whether or not vector2 a is within the Triumph range of vector2 b. So the Tile distance.
        /// </summary>
        /// <param Name="a">The first position from which to calculate the distance from.</param>
        /// <param Name="b">The second position from which to calculate the distance from.</param>
        /// <param Name="range">The range from which you want to check.</param>
        /// <returns></returns>
        public static bool IsTileWithinRange(Vector2 a, Vector2 b, int range)
        {
            // NOTE!!!!!!!
            // Because are sprites are a certain size. The location of the tiles is always in increments of 2 or negative 2. So Tile(1,1) is on 0,0,0. Tile(2,1) is on 2,0,0. 
            // Tile(1,2) is on 0,-2,0 etc.
            // Because of these increments, 2 unity meters equals to 1 range. When we say attackrange of 1 you can attack units directly beside you.
            // So thats why we devide the distance between the two units by 2 in order to get the correct distance.
            // If we later have different sprite sizes and/or different increments this needs to change.
            // This works for all ranges.

            float distance = Vector2.Distance(a, b);
            float tileDistance = distance/2;

            return tileDistance <= range;
        }

        /// <summary>
        /// This method returns all the tiles that are within a certain range calculated from the Tile which you specify.
        /// It takes into account out of bounds and doesn't add the Tile the which has the same coordinates as the given center point. 
        /// The return type is the same as the Tile collection in the GameManager for consistancy.
        /// The movement and attack classes / implementations should be responsible for further calculations.
        /// So if you want to Highlight possible attack locations the attack class / method should loop through all tiles in range
        /// and only hightlight the tiles on which an enemy unit is placed.
        /// </summary>
        /// <param Name="centerPointTileCoordinate">The tilecoordinate from which the calculation is done. </param>
        /// <param Name="range">The range from which tiles get returned</param>
        public static Dictionary<int, Dictionary<int, Tile>> GetAllTilesWithinRange(
            TileCoordinates centerPointTileCoordinate, int range)
        {
            GameManager _manager = GameObject.Find("_Scripts").GetComponent<GameManager>();
            // Check if the range is 0 or smaller.
            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException("range",
                    "The entered range is 0 or smaller. Please use a correct range");
            }

            if (!_manager.Tiles.ContainsKey(centerPointTileCoordinate.ColumnId) ||
                !_manager.Tiles[centerPointTileCoordinate.ColumnId].ContainsKey(centerPointTileCoordinate.RowId))
            {
                throw new ArgumentOutOfRangeException("centerPointTileCoordinate",
                    "The given center Tile does not exist. Please give a valid TileCoordinate");
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
                    // Get the Tile from the Tile list and add it to the return list.
                    Tile t = GetTile(new TileCoordinates(currentColumnId, currentRowid));
                    if (!possibleLocations.ContainsKey(currentColumnId))
                    {
                        possibleLocations.Add(currentColumnId, new Dictionary<int, Tile>());
                    }
                    possibleLocations[currentColumnId].Add(currentRowid, t);
                    currentRowid++;
                }
                currentColumnId++;
                // Determine if the currentColumnId has reached the center Tile columnid, ifso start making the size smaller.
                size = currentColumnId <= columnId ? size += 1 : size -= 1;
            }
            return possibleLocations;
        }

        /// <summary>
        /// Returns all of the tiles that are withing the players LOS. So if a unit or building has vision on a Tile it returns it.
        /// </summary>
        /// <param Name="index"></param>
        /// <returns></returns>
        public static List<Tile> GetAllTilesWithPlayerLOS(PlayerIndex index)
        {
            GameManager _manager = GameObject.Find("_Scripts").GetComponent<GameManager>();
            List<Tile> tileInLOSRange = new List<Tile>();
            Player player = _manager.Players[index];

            foreach (Unit unit in player.OwnedUnits)
            {
                tileInLOSRange.Add(unit.UnitGameObject.Tile);
                Dictionary<int, Dictionary<int, Tile>> tilesInRange =
                    TileHelper.GetAllTilesWithinRange(unit.UnitGameObject.Tile.Coordinate, unit.FowLineOfSightRange);
                foreach (KeyValuePair<int, Dictionary<int, Tile>> item in tilesInRange)
                {
                    foreach (KeyValuePair<int, Tile> tileValue in item.Value)
                    {
                        if (!tileInLOSRange.Contains(tileValue.Value))
                        {
                            tileInLOSRange.Add(tileValue.Value);
                        }
                    }
                }
            }
            foreach (Building building in player.OwnedBuildings)
            {
                tileInLOSRange.Add(building.BuildingGameObject.Tile);
                Dictionary<int, Dictionary<int, Tile>> tilesInRange =
                    TileHelper.GetAllTilesWithinRange(building.BuildingGameObject.Tile.Coordinate,
                        building.FowLineOfSightRange);
                foreach (KeyValuePair<int, Dictionary<int, Tile>> item in tilesInRange)
                {
                    foreach (KeyValuePair<int, Tile> tileValue in item.Value)
                    {
                        if (!tileInLOSRange.Contains(tileValue.Value))
                        {
                            tileInLOSRange.Add(tileValue.Value);
                        }
                    }
                }
            }
            return tileInLOSRange;
        }

        public static List<Tile> GetAllTilesInListType()
        {
            GameManager _manager = GameObject.Find("_Scripts").GetComponent<GameManager>();
            List<Tile> allTiles = new List<Tile>();
            foreach (KeyValuePair<int, Dictionary<int, Tile>> item in _manager.Tiles)
            {
                foreach (KeyValuePair<int, Tile> tileValue in item.Value)
                {
                    allTiles.Add(tileValue.Value);
                }
            }
            return allTiles;
        }
    }
}