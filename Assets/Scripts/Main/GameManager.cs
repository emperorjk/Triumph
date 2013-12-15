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
    private Dictionary<int, Dictionary<int, Tile>> tiles;
    private Dictionary<PlayerIndex, Player> players;
    private bool isAudioOn = true;
    private bool isQuitMenuOn = false;
    private Player currentPlayer;
    private int currentTurn = 1;
    private TextMesh currentTurnText;
    private TextMesh playerText;
    public bool isDoneButtonActive = false;

    /// <summary>
    /// Use this method as a constructor which is called once when the GameManager singleton is called for the first time.
    /// </summary>
    private void Init()
    {
        tiles = new Dictionary<int, Dictionary<int, Tile>>();
        players = new Dictionary<PlayerIndex, Player>();
        players.Add(PlayerIndex.One, new Player("Player 1"));
        players.Add(PlayerIndex.Two, new Player("Player 2"));

        currentPlayer = players[PlayerIndex.One];

        // getting the TextMesh components and setting the player name + current turn
        playerText = GameObject.Find("PlayerName").gameObject.GetComponent<TextMesh>();
        playerText.text = "Player: " + currentPlayer.name;

        currentTurnText = GameObject.Find("Turn").gameObject.GetComponent<TextMesh>();
        currentTurnText.text = "Turn: " + currentTurn.ToString();
    }

    #region menubar
    public void ChangeAudio(bool audio)
    {
        isAudioOn = audio;
    }

    public bool IsAudioOn()
    {
        return isAudioOn;
    }

    public void ChangeQuitMenuOn(bool quitMenu)
    {
        isQuitMenuOn = quitMenu;
    }

    public bool IsQuitMenuOn()
    {
        return isQuitMenuOn;
    }
    #endregion

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
        throw new KeyNotFoundException("The Tile was not found given the specified coordinates, YOU FOOL!!");
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

    public void NextPlayer()
    {
        currentTurn++;
        currentTurnText.text = "Turn: " + currentTurn.ToString();

        if (currentPlayer == players[PlayerIndex.One])
        {
            currentPlayer = players[PlayerIndex.Two];
            playerText.text = "Player: " + currentPlayer.name; 
        }
        else 
        {
            currentPlayer = players[PlayerIndex.One];
            playerText.text = "Player: " + currentPlayer.name; 
        }
    }
}
