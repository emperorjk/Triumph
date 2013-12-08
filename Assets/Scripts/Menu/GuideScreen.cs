using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideScreen : MonoBehaviour
{
    public GameObject forwardButton;
    public GameObject backwardButton;

    private RaycastHit touchBox;
    private int activeScreen;

    void Update()
    {
        if (MenuManager.activeMenuState == MenuStates.GuideState)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                MenuManager.Instance.BackToMenu();
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out touchBox))
                {
                    if (touchBox.collider == forwardButton.collider)
                    {
                        if (activeScreen == 0)
                        {
                            activeScreen++;
                            GameObject.Find("CaptureZoneGuide").renderer.enabled = true;
                        }
                        else if (activeScreen == 1)
                        {
                            activeScreen++;
                            GameObject.Find("CaptureZoneGuide").renderer.enabled = false;
                            GameObject.Find("TilesGuide").renderer.enabled = true;
                            GameObject.Find("ForwardButton").renderer.enabled = false;
                        }
                    }
                    else if (touchBox.collider == backwardButton.collider)
                    {
                        if (activeScreen == 0)
                        {
                            MenuManager.Instance.BackToMenu();
                        }
                        else if (activeScreen == 1)
                        {
                            activeScreen--;
                            GameObject.Find("CaptureZoneGuide").renderer.enabled = false;
                            GameObject.Find("TilesGuide").renderer.enabled = false;
                        }
                        else if (activeScreen == 2)
                        {
                            activeScreen--;
                            GameObject.Find("CaptureZoneGuide").renderer.enabled = true;
                            GameObject.Find("TilesGuide").renderer.enabled = false;
                            GameObject.Find("ForwardButton").renderer.enabled = true;
                        }
                    }
                }
            }
        }
    }
}
