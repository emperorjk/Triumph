using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScreen : MonoBehaviour 
{
    public List<GameObject> levels;
    private RaycastHit touchBox;

	void Update () 
    {
        if (MenuManager.activeMenuState == MenuStates.LevelState)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out touchBox))
                {
                    foreach (GameObject level in levels)
                    {
                        if (touchBox.collider == level.collider)
                        {
                            Debug.Log(level.name);

                            if (level.name == "level1")
                            {
                                Application.LoadLevel(1);
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuManager.Instance.BackToMenu();
        }
	}
}
