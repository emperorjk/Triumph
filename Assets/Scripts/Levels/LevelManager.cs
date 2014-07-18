using UnityEngine;
using SimpleJSON;

namespace Assets.Scripts.Levels
{
    public static class LevelManager
    {
        public static Level CurrentLevel { get; set; }

        public static void LoadLevel(LevelsEnum level)
        {
            if (level 	== LevelsEnum.Menu)
            {
                CurrentLevel = new Level(false, 0, 0, 0, 0, "Menu", "The uber main menu.");
                Application.LoadLevel(level.ToString());
            }
            else if (Application.loadedLevelName != level.ToString() && IsValidLevel(level))
            {
                string jsonString = Resources.Load<TextAsset>("JSON/Levels/" + level.ToString()).text;
                JSONNode jsonLevel = JSON.Parse(jsonString);

                int morning = Mathf.Clamp(jsonLevel["turn-morning"].AsInt, 1, int.MaxValue);
                int midday = Mathf.Clamp(jsonLevel["turn-midday"].AsInt, 1, int.MaxValue);
                int evening = Mathf.Clamp(jsonLevel["turn-evening"].AsInt, 1, int.MaxValue);
                int night = Mathf.Clamp(jsonLevel["turn-night"].AsInt, 1, int.MaxValue);
                string name = jsonLevel["level-name"];
                string description = jsonLevel["level-description"];

                Level levelLoaded = new Level(true, morning, midday, evening, night, name, description);
                CurrentLevel = levelLoaded;
                Application.LoadLevel(level.ToString());
            }
        }

        private static bool IsValidLevel(LevelsEnum level)
        {
            foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                if (level.ToString() == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}