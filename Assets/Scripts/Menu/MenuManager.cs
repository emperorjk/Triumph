using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Menu
{
    public class MenuManager
    {
        private static MenuManager instance;
        public Dictionary<MenuStates, GameObject> MenuPositions { get; private set; }
        public MenuStates ActiveMenuState { get; set; }

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

        public void Positions()
        {
            MenuPositions = new Dictionary<MenuStates, GameObject>();

            GameObject startScreen = GameObject.FindGameObjectWithTag("BackgroundStart");
            GameObject levelScreen = GameObject.FindGameObjectWithTag("BackgroundLevel");
            GameObject guideScreen = GameObject.FindGameObjectWithTag("BackgroundGuide");
            GameObject creditsScreen = GameObject.FindGameObjectWithTag("BackgroundCredits");

            MenuPositions.Add(MenuStates.StartState, startScreen);
            MenuPositions.Add(MenuStates.LevelState, levelScreen);
            MenuPositions.Add(MenuStates.GuideState, guideScreen);
            MenuPositions.Add(MenuStates.CreditsScreen, creditsScreen);
        }

        public void ChangeMenuScreen(MenuStates menu)
        {
            SetScripts(ActiveMenuState, false);
            ActiveMenuState = menu;
            SetScripts(menu, true);
            Camera.main.transform.position = MenuPositions[menu].renderer.bounds.center - new Vector3(0, 0, 1);
        }

        private void SetScripts(MenuStates state, bool active)
        {
            if (state == MenuStates.StartState)
            {
                MenuPositions[state].transform.parent.GetComponent<StartScreen>().enabled = active;
            }
            else if (state == MenuStates.GuideState)
            {
                MenuPositions[state].transform.parent.GetComponent<GuideScreen>().enabled = active;
            }
            else if (state == MenuStates.LevelState)
            {
                MenuPositions[state].transform.parent.GetComponent<LevelScreen>().enabled = active;
            }
            else if (state == MenuStates.CreditsScreen)
            {
                MenuPositions[state].transform.parent.GetComponent<CreditScreen>().enabled = active;
            }
        }
    }
}