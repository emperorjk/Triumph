using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Menu
{
    public class MenuManager
    {
        private static MenuManager instance;
        public Dictionary<MenuStates, GameObject> menuPositions { get; private set; }
        public MenuStates activeMenuState { get; set; }

        public static MenuManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuManager();
                }
                return instance;
            }
        }

        public void MenuPositions()
        {
            menuPositions = new Dictionary<MenuStates, GameObject>();

            GameObject startScreen = GameObject.FindGameObjectWithTag("BackgroundStart");
            GameObject levelScreen = GameObject.FindGameObjectWithTag("BackgroundLevel");
            GameObject guideScreen = GameObject.FindGameObjectWithTag("BackgroundGuide");
            GameObject creditsScreen = GameObject.FindGameObjectWithTag("BackgroundCredits");

            menuPositions.Add(MenuStates.StartState, startScreen);
            menuPositions.Add(MenuStates.LevelState, levelScreen);
            menuPositions.Add(MenuStates.GuideState, guideScreen);
            menuPositions.Add(MenuStates.CreditsScreen, creditsScreen);
        }

        public void ChangeMenuScreen(MenuStates menu)
        {
            SetScripts(activeMenuState, false);
            activeMenuState = menu;
            SetScripts(menu, true);
            Camera.main.transform.position = menuPositions[menu].renderer.bounds.center - new Vector3(0, 0, 1);
        }

        private void SetScripts(MenuStates state, bool active)
        {
            if (state == MenuStates.StartState)
            {
                menuPositions[state].transform.parent.GetComponent<StartScreen>().enabled = active;
            }
            else if (state == MenuStates.GuideState)
            {
                menuPositions[state].transform.parent.GetComponent<GuideScreen>().enabled = active;
            }
            else if (state == MenuStates.LevelState)
            {
                menuPositions[state].transform.parent.GetComponent<LevelScreen>().enabled = active;
            }
            else if (state == MenuStates.CreditsScreen)
            {
                menuPositions[state].transform.parent.GetComponent<CreditScreen>().enabled = active;
            }
        }
    }
}