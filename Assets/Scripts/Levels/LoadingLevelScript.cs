using UnityEngine;
using System.Collections;
using Assets.Scripts.Levels;
using SimpleJSON;
using System.Collections.Generic;
using Assets.Scripts.Players;
using System;
using Assets.Scripts.Main;

namespace Assets.Scripts.Levels
{

    public class LoadingLevelScript : MonoBehaviour
    {

        void Start()
        {
            LoadLevel();
        }


        /// <summary>
        /// Load the level that is set in the LevelManager script.
        /// </summary>
        private void LoadLevel()
        {
            LevelManager lm = GameObjectReferences.getGlobalScriptsGameObject().GetComponent<LevelManager>();
            LevelsEnum level = lm.levelToLoad;

            Debug.Log("Loading level in loading screen: " + level.ToString());

            if (level == LevelsEnum.Menu)
            {
                lm.CurrentLevel = new Level(level);
                Application.LoadLevel(level.ToString());
            }
            else
            {
                string jsonString = ResourceCache.getResource<TextAsset>(level.ToString()).text;

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

                lm.CurrentLevel = new Level(true, morning, midday, evening, night, name, description, startGold, level);
                Application.LoadLevel(level.ToString());
            }
        }
    }
}