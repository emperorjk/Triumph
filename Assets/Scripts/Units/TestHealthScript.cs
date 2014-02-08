using UnityEngine;
using System.Collections;

public class TestHealthScript : MonoBehaviour {

    private UnitGameObject unit;

	// Use this for initialization
	void Start () {
        unit = transform.parent.GetComponent<UnitGameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.SetFloat("_Cutoff", 1 - (unit.UnitGame.CurrentHealth / 10f));
	}
}
