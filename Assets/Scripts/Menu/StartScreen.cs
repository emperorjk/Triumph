using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class StartScreen : MonoBehaviour
    {
        public GameObject guideButton;
        public GameObject startButton;
        public GameObject creditButton;

        private RaycastHit touchBox;

        private void Start()
        {
            MenuManager.Instance.MenuPositions();
            MenuManager.Instance.ChangeMenuScreen(MenuStates.StartState);
        }

        private void Update()
        {
            ButtonClick();
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
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.LevelState);
                        GameObject.Find("StartButton").audio.Play();
                    }
                    else if (touchBox.collider == guideButton.collider)
                    {
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.GuideState);
                        GameObject.Find("GuideButton").audio.Play();
                    }
                    else if (touchBox.collider == creditButton.collider)
                    {
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.CreditsScreen);
                    }
                }
            }
        }
    }
}