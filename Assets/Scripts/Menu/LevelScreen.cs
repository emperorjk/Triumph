using Assets.Scripts.Levels;
using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.Main;

namespace Assets.Scripts.Menu
{
    public class LevelScreen : MonoBehaviour
    {
        public List<GameObject> levels;
        public GameObject backButton;

        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit)
                {
                    foreach (GameObject level in levels)
                    {
                        if (hit.collider == level.collider2D)
                        {
                            LoadLevel(level.name);
                        }
                    }

                    if (hit.collider == backButton.collider2D)
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
                    GameObjectReferences.getGlobalScriptsGameObject().GetComponent<LevelManager>().LoadLevel(level);
                    break;
                }
            }
        }
    }
}