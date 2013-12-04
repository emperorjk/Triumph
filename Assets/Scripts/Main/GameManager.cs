using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameManager
{
    #region Singleton
    private static GameManager instance;
    private GameManager() { }
    public static GameManager Instance { 
        get {
            if (Instance == null) 
            { 
                instance = new GameManager();
                instance.Init();
            }
            return instance;
        }
    }
    #endregion
    private IList<Tile> tiles;
    private IList<Player> players;

    /// <summary>
    /// Use this method as a constructor which is called once when the GameManager singleton is called for the first time.
    /// </summary>
    private void Init()
    {
        tiles = new List<Tile>();
        players = new List<Player>();
    }

    /// <summary>
    /// Returns the tile which the unit is placed on. Returns null if it cannot find a tile.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public Tile GetTile(UnitBase unit)
    {
        return tiles.First(x => x.unit.Equals(unit));
    }

    /// <summary>
    /// Returns the tile which the building is placed on. Returns null if it cannot find a tile.
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    public Tile GetTile(BuildingsBase building)
    {
        return tiles.First(x => x.building.Equals(building));
    }

    /// <summary>
    /// Returns the tile which the environment is placed on. Returns null if it cannot find a tile.
    /// </summary>
    /// <param name="environment"></param>
    /// <returns></returns>
    public Tile GetTile(EnvironmentBase environment)
    {
        return tiles.First(x => x.environment.Equals(environment));
    }

    public Player GetPlayer(string name)
    {
        return players.First(x => x.name.Equals(name));
    }
}
