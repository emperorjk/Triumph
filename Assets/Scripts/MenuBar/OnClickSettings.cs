using UnityEngine;
using System.Collections;

public class OnClickSettings : MonoBehaviour 
{

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (!GameManager.Instance.isQuitMenuOn)
            {
                this.gameObject.transform.FindChild("Quit").gameObject.SetActive(true);
                GameManager.Instance.isQuitMenuOn = true;
            }
            else if (GameManager.Instance.isQuitMenuOn)
            {
                this.gameObject.transform.FindChild("Quit").gameObject.SetActive(false);
                GameManager.Instance.isQuitMenuOn = false;
            }
        }
    }
}
