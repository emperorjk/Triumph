using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	void Start () 
    {
		transform.position = GameObject.FindGameObjectWithTag("BackgroundStart").renderer.bounds.center - new Vector3(0,0,1);
	}
}
