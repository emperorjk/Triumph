using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

/// <summary>
/// This singleton class keeps hold of the game.
/// </summary>
public class GameManager
{
    #region Singleton
    private static GameManager instance;
    private GameManager() { }
    public static GameManager Instance { 
        get 
        {
            if (instance == null) 
            { 
                instance = new GameManager();
                instance.Init();
            }
            return instance;
        }
    }
    #endregion

    public Dictionary<int, Dictionary<int, Tile>> tiles;
    public bool IsAudioOn { get; set; }
    public bool IsQuitMenuOn { get; set; }
    public bool IsDoneButtonActive { get; set; }
    public bool IsProductionOverlayActive { get; set; }
    public Player CurrentPlayer { get; set; }
    public bool NeedMoving { get; set; }
    public Tile LastClickedBuildingTile { get; set; }
    public GameObject LastClickedUnitGO { get; set; }
    public Tile LastClickedUnitTile { get; set; }
    public bool IsHightlightOn { get; set; }
    public CaptureBuildings CaptureBuildings { get; private set; }
    public bool UnitCanAttack { get; set; }

    // Lists need to be accesed in GameManager because when NextPlayer method gets called we want to deactivate
    // the highlights also.
    public List<GameObject> highLightObjects = new List<GameObject>();
    public List<GameObject> attackHighLightObjects = new List<GameObject>();

    // The Player object can still be retrieved via the PlayerIndex enum.
    private SortedList<PlayerIndex, Player> players;
    private int currentTurn = 1;
    private TextMesh currentTurnText;
    private TextMesh playerText;
    private TextMesh currentGold;

    /// <summary>
    /// Use this method as a constructor which is called once when the GameManager singleton is called for the first time.
    /// </summary>
    private void Init()
    {	
		IsAudioOn = true;
        IsProductionOverlayActive = false;
        tiles = new Dictionary<int, Dictionary<int, Tile>>();
        players = new SortedList<PlayerIndex, Player>();
        players.Add(PlayerIndex.Neutral, new Player("Neutral player", PlayerIndex.Neutral));
        players.Add(PlayerIndex.One, new Player("Player 1", PlayerIndex.One));
        players.Add(PlayerIndex.Two, new Player("Player 2", PlayerIndex.Two));

        CurrentPlayer = players[PlayerIndex.One];
        CaptureBuildings = new CaptureBuildings();
    }

    /// <summary>
    /// Add a tile to the list. This methods should only be called one when a Tile GameObject is loaded when the scene starts.
    /// </summary>
    /// <param name="tile"></param>
    public void AddTile(Tile tile)
    {
        // Check if the second dictionary exists in the list. If not then create a new dictionary and insert this in the tiles dictionary.
        if(!tiles.ContainsKey(tile.coordinate.ColumnId))
        {
            tiles.Add(tile.coordinate.ColumnId, new Dictionary<int, Tile>());
        }
        // Last insert the tile object into the correct spot in the dictionarys. Since we now know that both dictionary at these keys exist.
        tiles[tile.coordinate.ColumnId].Add(tile.coordinate.RowId, tile);
    }

    /// <summary>
    /// Removes all entrys from the tiles dictionary. Should be called before a new scene (level) starts to load. So no old references exists when new Tile are added to the list.
    /// </summary>
    public void ClearTilesDictionary()
    {
        tiles.Clear();
    }

    /// <summary>
    /// Returns the tile with via the given TileCoordinates from the tiles dictionary. Or an KeyNotFoundException if either of the keys is not found.
    /// </summary>
    /// <param name="coor"></param>
    /// <returns></returns>
    public Tile GetTile(TileCoordinates coor)
    {
        if(tiles.ContainsKey(coor.ColumnId) && tiles[coor.ColumnId].ContainsKey(coor.RowId))
        {
            return tiles[coor.ColumnId][coor.RowId];
        }

        return null;
    }

    /// <summary>
    /// Returns the player object by the given PlayerIndex enum.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Player GetPlayer(PlayerIndex index)
    {
        if(!Enum.IsDefined(typeof(PlayerIndex), index))
        {
            throw new KeyNotFoundException("The given playerIndex was not found. Give me a correct PlayerIndex or suffer the consequences.");
        }
        return players[index];
    }
	
	/// <summary>
    /// Perhaps a more cleaner system where a class holds all of the level specific data. For now this method needs to be called whenever a level scene is loaded. If loaded from
    /// the menu errors will come forth.
    /// </summary>
    public void SetupLevel()
    {
        // getting the TextMesh components and setting the player name + current turn
        playerText = GameObject.Find("PlayerName").gameObject.GetComponent<TextMesh>();
        playerText.text = "Player: " + CurrentPlayer.name;

        currentTurnText = GameObject.Find("Turn").gameObject.GetComponent<TextMesh>();
        currentTurnText.text = "Turn: " + currentTurn.ToString();

        currentGold = GameObject.Find("CurrentGold").gameObject.GetComponent<TextMesh>();
        currentGold.text = "Current gold: " + CurrentPlayer.gold;
    }

    public void NextPlayer()
    {
        ClearMovementAndHighLights();

        // calculate all of the buildings that are being captured.
        CaptureBuildings.CalculateCapturing();
        CurrentPlayer.IncreaseGoldBy(CurrentPlayer.GetCurrentIncome());
        
        // Change the currentplayer to the next player. Works with all amount of players. Ignores the Neutral player.
        bool foundPlayer = false;
        while(!foundPlayer)
        {
            int indexplayer = players.IndexOfKey(CurrentPlayer.index) + 1;
            if (indexplayer >= players.Count) { indexplayer = 0; }
            CurrentPlayer = players.Values[indexplayer];
            foundPlayer = CurrentPlayer.index != PlayerIndex.Neutral;
        }
        
        UpdateTextboxes();
    }

    public void UpdateTextboxes()
    {
        playerText.text = "Player: " + CurrentPlayer.name;
        currentGold.text = "Current gold: " + CurrentPlayer.gold;
        currentTurn++;
        currentTurnText.text = "Turn: " + currentTurn.ToString();
    }

    void ClearMovementAndHighLights()
    {
        foreach (UnitBase unit in CurrentPlayer.ownedUnits)
        {
            unit.unitGameObject.renderer.material.color = Color.white;
            unit.hasMoved = false;
            unit.hasAttacked = false;
        }

        foreach (GameObject highlights in GameManager.Instance.highLightObjects)
        {
            highlights.SetActive(false);
        }

        foreach (GameObject attackHighlight in GameManager.Instance.attackHighLightObjects)
        {
            attackHighlight.SetActive(false);
        }
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

        if (!tiles.ContainsKey(centerPointTileCoordinate.ColumnId) || !tiles[centerPointTileCoordinate.ColumnId].ContainsKey(centerPointTileCoordinate.RowId))
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
            if (!tiles.ContainsKey(currentColumnId))
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
                if (!tiles[currentColumnId].ContainsKey(currentRowid) ||
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
