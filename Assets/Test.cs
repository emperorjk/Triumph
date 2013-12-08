using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    private bool hasRun = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // After two seconds deactivate a tile from the level.
	    if(Time.realtimeSinceStartup > 2f && !hasRun)
        {
            Debug.Log("Testing script. DeActivating a tile.");
            hasRun = true;
            TileCoordinates c = new TileCoordinates(11, 6);
            Debug.Log("ColumnId: " + c.ColumnId);
            Debug.Log("RowId: " + c.RowId);
            Tile t = GameManager.Instance.GetTile(c);
            t.gameObject.SetActive(false);
        }
	}
}
