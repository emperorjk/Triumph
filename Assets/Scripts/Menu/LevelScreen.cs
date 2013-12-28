using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScreen : MonoBehaviour 
{
    public List<GameObject> levels;
    private RaycastHit touchBox;
    public GameObject backButton;

	void Update () 
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
                        LoadLevel(level.name);
                    }
                }

                if (touchBox.collider == backButton.collider)
                {
                    MenuManager.Instance.ChangeMenuScreen(MenuStates.StartState);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Find("Level").GetComponents<AudioSource>()[1].Play();
            MenuManager.Instance.ChangeMenuScreen(MenuStates.StartState);
        }
	}

    void LoadLevel(string name)
    {
        GameObject.Find("Level").GetComponents<AudioSource>()[0].Play();
        Application.LoadLevel(name);
    }
}
