using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using Assets.Scripts.Players;
using System;

namespace Assets.Scripts.Levels
{
    public class LevelManager : MonoBehaviour
    {
        public static Level CurrentLevel { get; set; }

        public static void LoadLevel(LevelsEnum level)
        {
            CurrentLevel = null;
            if (level == LevelsEnum.Menu)
            {
                CurrentLevel = new Level();
                Application.LoadLevel(level.ToString());
            }
            else
            {
                string jsonString = Resources.Load<TextAsset>("JSON/Levels/" + level).text;
                JSONNode jsonLevel = JSON.Parse(jsonString);

                int morning = Mathf.Clamp(jsonLevel["turn-morning"].AsInt, 1, int.MaxValue);
                int midday = Mathf.Clamp(jsonLevel["turn-midday"].AsInt, 1, int.MaxValue);
                int evening = Mathf.Clamp(jsonLevel["turn-evening"].AsInt, 1, int.MaxValue);
                int night = Mathf.Clamp(jsonLevel["turn-night"].AsInt, 1, int.MaxValue);
                string name = jsonLevel["level-name"];
                string description = jsonLevel["level-description"];
                JSONArray gold = jsonLevel["starting-gold"].AsArray;

                Dictionary<PlayerIndex, int> startGold = new Dictionary<PlayerIndex, int>();

                foreach (PlayerIndex pl in (PlayerIndex[])Enum.GetValues(typeof(PlayerIndex)))
                {
                    foreach (JSONNode item in gold)
                    {
                        if (!String.IsNullOrEmpty(item[pl.ToString()]) && item[pl.ToString()] != pl.ToString())
                        {
                            startGold.Add(pl, item[pl.ToString()].AsInt);
                        }
                    }
                }

                Level levelLoaded = new Level(true, morning, midday, evening, night, name, description, startGold, level);
                Debug.Log(CurrentLevel);
                CurrentLevel = levelLoaded;
                Debug.Log(CurrentLevel);
                Application.LoadLevel(level.ToString());
            }
        }
    }
}