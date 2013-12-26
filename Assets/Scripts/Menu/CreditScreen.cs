using UnityEngine;
using System.Collections;

public class CreditScreen : MonoBehaviour {
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuManager.Instance.ChangeMenuScreen(MenuStates.StartState);
        }
	}
}
