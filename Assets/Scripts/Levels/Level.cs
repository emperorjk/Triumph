using System.Collections.Generic;
using Assets.Scripts.DayNight;
using Assets.Scripts.Players;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class Level
    {
        public Dictionary<int, Dictionary<int, Tile>> Tiles { get; private set; }
        public Player CurrentPlayer { get; set; }
        public SortedList<PlayerIndex, Player> Players { get; private set; }

        public string levelName;
        public string levelDescription;
        public bool IsEnded;
        public LevelsEnum type;

        /// <summary>
        /// Indicates wether the level is a playable ingame level. If false then it is something like a menu, loading screen, ending screen or some sort of other level.
        /// </summary>
        public bool isIngameLevel;
        public Dictionary<DayStates, int> dayNightTurns;

        public Level(bool isIngameLevel, int morning, int midday, int evening, int night, string name,
            string description, Dictionary<PlayerIndex, int> startingGold, LevelsEnum type)
        {
            dayNightTurns = new Dictionary<DayStates, int>();
            dayNightTurns.Add(DayStates.Morning, morning);
            dayNightTurns.Add(DayStates.Midday, midday);
            dayNightTurns.Add(DayStates.Evening, evening);
            dayNightTurns.Add(DayStates.Night, night);
            this.isIngameLevel = isIngameLevel;
            levelName = name;
            levelDescription = description;
            this.type = type;

            Tiles = new Dictionary<int, Dictionary<int, Tile>>();
            Players = new SortedList<PlayerIndex, Player>();
            Players.Add(PlayerIndex.Neutral, new Player("Neutral player", PlayerIndex.Neutral, Color.gray));
            Players.Add(PlayerIndex.Blue, new Player("Player Blue", PlayerIndex.Blue, Color.blue));
            Players.Add(PlayerIndex.Red, new Player("Player Red", PlayerIndex.Red, Color.red));
            CurrentPlayer = Players[PlayerIndex.Blue];
        }

        public Level() { }
    }
}