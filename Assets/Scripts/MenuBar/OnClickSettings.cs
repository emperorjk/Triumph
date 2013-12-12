using UnityEngine;
using System.Collections;

public class OnClickSettings : MonoBehaviour 
{

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("Settings knop, where is that image..");
        }
    }
}
