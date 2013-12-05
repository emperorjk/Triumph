using UnityEngine;
using System.Collections;

public class MenuManager 
{
    private static MenuManager instance;
    public Vector3[] menuPositions { get; private set; }

    public static MenuStates activeMenuState { get; set; }

    private MenuManager() 
    {
        MenuPositions();
    }

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

    private void MenuPositions()
    {
        menuPositions = new Vector3[3];
        Vector3 startScreen = GameObject.FindGameObjectWithTag("BackgroundStart").renderer.bounds.center;
        Vector3 levelScreen = GameObject.FindGameObjectWithTag("BackgroundLevel").renderer.bounds.center;
        Vector3 guideScreen = GameObject.FindGameObjectWithTag("BackgroundGuide").renderer.bounds.center;

        // set all z values to zero otherwise the camera has the same z value as the background image
        levelScreen.z = 0;
        guideScreen.z = 0;
        startScreen.z = 0;

        menuPositions[0] = startScreen;
        menuPositions[1] = levelScreen;
        menuPositions[2] = guideScreen;
    }
}
