using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour 
{

	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject.Find("_Scripts").GetComponent<GameManager>().LevelManager.LoadLevel(LevelsEnum.Menu);
        }
	}
}
