using UnityEngine;
using System;

namespace Assets.Scripts.Levels
{
    public class LevelManager : MonoBehaviour
    {
        private const String loadLevelSceneName = "LoadingLevelScene";

        public Level CurrentLevel { get; set; }

        public LevelsEnum levelToLoad { get; private set; }

        /// <summary>
        /// Load the level. It will load the loadingscene and from there load the properties of the level.
        /// </summary>
        /// <param name="level"></param>
        public void LoadLevel(LevelsEnum level)
        {
            levelToLoad = level;
            Application.LoadLevel(loadLevelSceneName);
        }

        public Boolean IsCurrentLevelLoaded()
        {
            return CurrentLevel != null;
        }
    }
}