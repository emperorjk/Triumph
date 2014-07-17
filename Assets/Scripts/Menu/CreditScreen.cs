using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class CreditScreen : MonoBehaviour
    {
        public GameObject backButton;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit)
                {
                    if (hit.collider == backButton.collider2D)
                    {
                        MenuManager.Instance.ChangeMenuScreen(MenuStates.StartState);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuManager.Instance.ChangeMenuScreen(MenuStates.StartState);
            }
        }
    }
}