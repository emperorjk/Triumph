using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class StartScreen : MonoBehaviour
    {
        public GameObject guideButton;
        public GameObject startButton;
        public GameObject creditButton;

        private RaycastHit touchBox;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out touchBox))
                {
                    if (touchBox.collider == startButton.collider)
                    {
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.LevelState);
                        start.audio.Play();
                    }
                    else if (touchBox.collider == guideButton.collider)
                    {
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.GuideState);
                        start.audio.Play();
                    }
                    else if (touchBox.collider == creditButton.collider)
                    {
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.CreditsScreen);
                        start.audio.Play();
                    }
                }
            }
        }
    }
}