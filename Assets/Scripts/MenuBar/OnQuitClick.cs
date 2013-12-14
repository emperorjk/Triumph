using UnityEngine;
using System.Collections;

public class OnQuitClick : MonoBehaviour 
{
    void OnMouseDown()
    {
        if (collider.name == "Yes")
        {
            GameManager.Instance.ChangeQuitMenuOn(false);
            Application.LoadLevel(0);
        }
        else if (collider.name == "No")
        {
            GameObject.Find("Quit").SetActive(false);
            GameManager.Instance.ChangeQuitMenuOn(false);
        }
    }


}
