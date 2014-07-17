using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Menu
{
    public class GuideScreen : MonoBehaviour
    {
        public List<Sprite> guideScreenSprites;
        public GameObject forwardButton;
        public GameObject backwardButton;

        private int activeScreen;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.Find("Guide").GetComponent<AudioSource>().Play();

                // reset images and activeScreen to 0
                activeScreen = 0;
                forwardButton.renderer.enabled = true;

                MenuManager.Instance.ChangeMenuScreen(MenuStates.StartState);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit)
                {
                    if (hit.collider == forwardButton.collider2D)
                    {
                        activeScreen++;
                        if (activeScreen >= guideScreenSprites.Count - 1)
                        {
                            activeScreen = guideScreenSprites.Count - 1;
                            forwardButton.renderer.enabled = false;
                        }

                        GameObject.Find("GuideScreen").GetComponent<SpriteRenderer>().sprite =
                            guideScreenSprites[activeScreen];
                    }
                    else if (hit.collider == backwardButton.collider2D)
                    {
                        activeScreen--;
                        if (activeScreen < 0)
                        {
                            MenuManager.Instance.ChangeMenuScreen(MenuStates.StartState);
                            activeScreen = 0;
                        }
                        if (activeScreen < guideScreenSprites.Count)
                        {
                            forwardButton.renderer.enabled = true;
                        }

                        GameObject.Find("GuideScreen").GetComponent<SpriteRenderer>().sprite =
                            guideScreenSprites[activeScreen];
                    }
                }  
            }
        }
    }
}