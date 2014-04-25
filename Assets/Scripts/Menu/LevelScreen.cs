using Assets.Scripts.Levels;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Menu
{
    public class LevelScreen : MonoBehaviour
    {
        public List<GameObject> levels;
        private RaycastHit touchBox;
        public GameObject backButton;

        private void Update()
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

        private void LoadLevel(string name)
        {
            GameObject.Find("Level").GetComponents<AudioSource>()[0].Play();

            foreach (LevelsEnum level in (LevelsEnum[]) Enum.GetValues(typeof (LevelsEnum)))
            {
                if (level.ToString() == name)
                {
                    GameObject.Find("_GlobalScripts").GetComponent<LevelManager>().LoadLevel(level);
                    break;
                }
            }
        }
    }
}