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
    private GameManager() {}
    public static GameManager Instance { 
        get 
        {
            if (instance == null) 
            { 
                instance = new GameManager();
                instance.InitPlayer();
            }
            return instance;
        }
    }
    #endregion

    // Objects
    public GameObject GlobalScripts { get; set; }
    public Dictionary<int, Dictionary<int, Tile>> Tiles { get; private set; }
    public Player CurrentPlayer { get; private set; }
    public SortedList<PlayerIndex, Player> Players { get; private set; }
    public ProductionOverlayMain ProductionOverlayMain { get; set; }
    public CaptureBuildings CaptureBuildings { get; set; }
    public DayStateController DayStateController { get; set; }
    public AudioManager UnitSounds { get; set; }
    public Highlight Highlight { get; set; }
    public Attack Attack { get; set; }
    public Movement Movement { get; set; }
    public AnimationInfo AnimInfo { get; set; }
    public LevelManager LevelManager { get; set; }

    // Variables
    public bool IsDoneButtonActive { get; set; }

    private void InitPlayer()
    {
        Tiles = new Dictionary<int, Dictionary<int, Tile>>();
        Players = new SortedList<PlayerIndex, Player>();
        Players.Add(PlayerIndex.Neutral, new Player("Neutral player", PlayerIndex.Neutral, Color.gray));
        Players.Add(PlayerIndex.Blue, new Player("Player Blue", PlayerIndex.Blue, Color.blue));
        Players.Add(PlayerIndex.Red, new Player("Player Red", PlayerIndex.Red, Color.red));
        CurrentPlayer = Players[PlayerIndex.Blue];
    }

    public void Init()
    {
        GameObject scriptsGameObject = GameObject.Find("_Scripts");
        ProductionOverlayMain = scriptsGameObject.GetComponent<ProductionOverlayMain>();
        Movement = scriptsGameObject.GetComponent<Movement>();
        AnimInfo = scriptsGameObject.GetComponent<AnimationInfo>();
        CaptureBuildings = scriptsGameObject.GetComponent<CaptureBuildings>();
        DayStateController = scriptsGameObject.GetComponent<DayStateController>();
        Highlight = scriptsGameObject.GetComponent<Highlight>();
        Attack = scriptsGameObject.GetComponent<Attack>();
        UnitSounds = new AudioManager();
        LevelManager = GlobalScripts.GetComponent<LevelManager>();
    }

    public void EndTurn()
    {
        if(!this.AnimInfo.IsAnimateFight && !this.Movement.NeedsMoving)
        {
            ProductionOverlayMain.DestroyAndStopOverlay();
            Highlight.ClearMovementAndHighLights();
            CaptureBuildings.CalculateCapturing();
            CurrentPlayer.IncreaseGoldBy(CurrentPlayer.GetCurrentIncome());

            // Change the currentplayer to the next player. Works with all amount of players. Ignores the Neutral player.
            bool foundPlayer = false;
            while (!foundPlayer)
            {
                int indexplayer = Players.IndexOfKey(CurrentPlayer.index) + 1;
                if (indexplayer >= Players.Count) { indexplayer = 0; }
                CurrentPlayer = Players.Values[indexplayer];
                foundPlayer = CurrentPlayer.index != PlayerIndex.Neutral;
            }

            // After end turn we want to loop through loots and IncreaseTurn so that loot will destroy after x amount turns.
            Loot[] loots = GameObject.FindObjectsOfType<Loot>();
            foreach (Loot l in loots)
            {
                l.IncreaseTurn();
            }
            
            DayStateController.TurnIncrease();
        }
    }

    /// <summary>
    /// This method is called from the gameloop void OnDestroy method. In here we clear all this that needs clearing and/or reset from the gamemanager.
    /// Make sure we set all of the values to zero so the garbace collector can remove all of these objects from memory.
    /// Otherwise everytime we start a new level we recreate them while the previous are still in memory.
    /// </summary>
    public void OnGameloopDestroy()
    {
        TileHelper.ClearTilesDictionary();
        // Clear all stuff from the player and reinitialize the player tile and player objects by calling the InitPlayer() method.
        foreach (Player pl in Players.Select(x => x.Value))
        {
            pl.ownedBuildings.Clear();
            pl.ownedUnits.Clear();
        }
        Players.Clear();
        InitPlayer();

        ProductionOverlayMain = null;
        Movement = null;
        AnimInfo = null;
        CaptureBuildings = null;
        DayStateController = null;
        Highlight = null;
        Attack = null;
        UnitSounds = null;
        LevelManager = null;
    }
}