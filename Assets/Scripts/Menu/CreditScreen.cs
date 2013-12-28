using UnityEngine;
using System.Collections;

public class CreditScreen : MonoBehaviour {
	
    private RaycastHit touchBox;
    public GameObject backButton;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out touchBox))
            {
                if (touchBox.collider == backButton.collider)
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
