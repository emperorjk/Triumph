using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour 
{
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Application.LoadLevel("Menu");
        }
	}
}
