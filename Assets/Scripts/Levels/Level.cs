using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct Level
{
    public string levelName;
    public string levelDescription;
    /// <summary>
    /// Indicates wether the level is a playable ingame level. If false then it is something like a menu, loading screen, ending screen or some sort of other level.
    /// </summary>
    public bool isIngameLevel;

    public Dictionary<DayStates, int> dayNightTurns;

    public Level(bool isIngameLevel, int morning, int midday, int evening, int night, string name, string description)
    {
        dayNightTurns = new Dictionary<DayStates, int>();
        dayNightTurns.Add(DayStates.Morning, morning);
        dayNightTurns.Add(DayStates.Midday, midday);
        dayNightTurns.Add(DayStates.Evening, evening);
        dayNightTurns.Add(DayStates.Night, night);
        this.isIngameLevel = isIngameLevel;
        levelName = name;
        levelDescription = description;
    }
}