using UnityEngine;
using System.Collections;

public class OnClickSettings : MonoBehaviour 
{

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (!GameManager.Instance.IsQuitMenuOn)
            {
                this.gameObject.transform.FindChild("Quit").gameObject.SetActive(true);
                GameManager.Instance.IsQuitMenuOn = true;
            }
            else if (GameManager.Instance.IsQuitMenuOn)
            {
                this.gameObject.transform.FindChild("Quit").gameObject.SetActive(false);
                GameManager.Instance.IsQuitMenuOn = false;
            }
        }
    }
}
