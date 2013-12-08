using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour
{
    public GameObject guideButton;
    public GameObject startButton;

    private MenuStates currentState = MenuStates.StartState;
    private RaycastHit touchBox;

	void Update () 
    {
        if (MenuManager.activeMenuState == MenuStates.StartState)
        {
            ButtonClick();
        }

	}

    private void ButtonClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out touchBox))
            {
                if (touchBox.collider == startButton.collider)
                {
                    Camera.main.transform.position = MenuManager.Instance.menuPositions[1];
                    MenuManager.activeMenuState = MenuStates.LevelState;
                }
                else if (touchBox.collider == guideButton.collider)
                {
                    Camera.main.transform.position = MenuManager.Instance.menuPositions[2];
                    MenuManager.activeMenuState = MenuStates.GuideState;
                }
            }
        }
    }
}
