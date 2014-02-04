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
            }
            return instance;
        }
    }
    #endregion


    // Objects
    public Dictionary<int, Dictionary<int, Tile>> Tiles { get; private set; }
    public Player CurrentPlayer { get; private set; }
    public SortedList<PlayerIndex, Player> Players { get; private set; }
    public ProductionOverlayMain ProductionOverlayMain { get; set; }
    public CaptureBuildings CaptureBuildings { get; set; }
    public FogOfWar Fow { get; set; }
    public UnitSounds UnitSounds { get; set; }
    public Highlight Highlight { get; set; }
    public Attack Attack { get; set; }
    public Movement Movement { get; set; }
    public AnimationInfo AnimInfo { get; set; }

    // Variables
    public int CurrentTurn { get; private set; }
    public bool IsAudioOn { get; set; }
    public bool IsQuitMenuOn { get; set; }
    public bool IsDoneButtonActive { get; set; }

    public void Init()
    {
        Tiles = new Dictionary<int, Dictionary<int, Tile>>();
        
        CurrentTurn = 1;
		IsAudioOn = true;
        Players = new SortedList<PlayerIndex, Player>();
        Players.Add(PlayerIndex.Neutral, new Player("Neutral player", PlayerIndex.Neutral));
        Players.Add(PlayerIndex.Blue, new Player("Player Blue", PlayerIndex.Blue));
        Players.Add(PlayerIndex.Red, new Player("Player Red", PlayerIndex.Red));
        CurrentPlayer = Players[PlayerIndex.Blue];

        ProductionOverlayMain = GameObject.Find("_Scripts").GetComponent<ProductionOverlayMain>();
        Movement = GameObject.Find("_Scripts").GetComponent<Movement>();
        AnimInfo = GameObject.Find("_Scripts").GetComponent<AnimationInfo>();
        CaptureBuildings = GameObject.Find("_Scripts").GetComponent<CaptureBuildings>();
        Fow = GameObject.Find("_Scripts").GetComponent<FogOfWar>();
        Highlight = GameObject.Find("_Scripts").GetComponent<Highlight>();
        Attack = GameObject.Find("_Scripts").GetComponent<Attack>();

        UnitSounds = new UnitSounds();
    }

    public void NextPlayer()
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
            CurrentTurn++;
            // Needs to be called after the CurrentTurn has increase in the UpdateTextBoxes() method. 
            Fow.ShowOrHideFowPlayer();
        }
    }
}