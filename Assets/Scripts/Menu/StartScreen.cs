using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class StartScreen : MonoBehaviour
    {
        public GameObject[] startButtons;

        private GameObject start;

        private void Start()
        {
            MenuManager.Instance.Positions();
            MenuManager.Instance.ChangeMenuScreen(MenuStates.StartState);
            start = GameObject.Find("Start");
        }

        private void Update()
        {
            ButtonClick();
        }

        private void ButtonClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit)
                {
                    if (hit.collider == startButtons[0].collider2D)
                    {
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.LevelState);
                    }
                    else if (hit.collider == startButtons[1].collider2D)
                    {
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.GuideState);
                    }
                    else if (hit.collider == startButtons[2].collider2D)
                    {
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.CreditsScreen);
                    }
                    start.audio.Play();
                }
            }
        }
    }
}