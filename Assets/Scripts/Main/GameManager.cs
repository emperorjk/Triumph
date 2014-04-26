using Assets.Scripts.Audio;
using Assets.Scripts.Buildings;
using Assets.Scripts.Controls;
using Assets.Scripts.DayNight;
using Assets.Scripts.Levels;
using Assets.Scripts.Players;
using Assets.Scripts.Production;
using Assets.Scripts.Tiles;
using Assets.Scripts.UnitActions;
using Assets.Scripts.Units;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using AnimationInfo = Assets.Scripts.UnitActions.AnimationInfo;

namespace Assets.Scripts.Main
{
    /// <summary>
    /// This singleton class keeps hold of the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
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
        public SwipeController SwipeController { get; set; }
        public bool IsDoneButtonActive { get; set; }

        private void Awake()
        {
            Tiles = new Dictionary<int, Dictionary<int, Tile>>();
            Players = new SortedList<PlayerIndex, Player>();
            Players.Add(PlayerIndex.Neutral, new Player("Neutral player", PlayerIndex.Neutral, Color.gray));
            Players.Add(PlayerIndex.Blue, new Player("Player Blue", PlayerIndex.Blue, Color.blue));
            Players.Add(PlayerIndex.Red, new Player("Player Red", PlayerIndex.Red, Color.red));
            CurrentPlayer = Players[PlayerIndex.Blue];

            GameObject scriptsGameObject = GameObject.Find("_Scripts");
            ProductionOverlayMain = scriptsGameObject.GetComponent<ProductionOverlayMain>();
            Movement = scriptsGameObject.GetComponent<Movement>();
            AnimInfo = scriptsGameObject.GetComponent<AnimationInfo>();
            CaptureBuildings = scriptsGameObject.GetComponent<CaptureBuildings>();
            DayStateController = scriptsGameObject.GetComponent<DayStateController>();
            Highlight = scriptsGameObject.GetComponent<Highlight>();
            Attack = scriptsGameObject.GetComponent<Attack>();
            UnitSounds = new AudioManager();

            GameObject globalScripts = GameObject.Find("_Scripts");
            LevelManager = globalScripts.GetComponent<LevelManager>();
            SwipeController = globalScripts.GetComponent<SwipeController>();
        }

        public void EndTurn()
        {
            if (!this.AnimInfo.IsAnimateFight && !this.Movement.NeedsMoving)
            {
                ProductionOverlayMain.DestroyAndStopOverlay();
                Highlight.ClearMovementAndHighLights();
                CaptureBuildings.CalculateCapturing();
                CurrentPlayer.IncreaseGoldBy(CurrentPlayer.GetCurrentIncome());

                // Change the currentplayer to the next player. Works with all amount of players. Ignores the Neutral player.
                bool foundPlayer = false;
                while (!foundPlayer)
                {
                    int indexplayer = Players.IndexOfKey(CurrentPlayer.Index) + 1;
                    if (indexplayer >= Players.Count)
                    {
                        indexplayer = 0;
                    }
                    CurrentPlayer = Players.Values[indexplayer];
                    foundPlayer = CurrentPlayer.Index != PlayerIndex.Neutral;
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

        private void OnDestroy()
        {
            Tiles.Clear();
            // Clear all stuff from the player and reinitialize the player Tile and player objects by calling the InitPlayer() method.
            foreach (Player pl in Players.Select(x => x.Value))
            {
                pl.OwnedBuildings.Clear();
                pl.OwnedUnits.Clear();
            }
            Players.Clear();

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
}