using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {

	void Start () 
    {
	
	}
	
	void Update () 
    {
        if (Input.touchCount >= 2)
        {
            Debug.Log("test");
        }
	}
}
