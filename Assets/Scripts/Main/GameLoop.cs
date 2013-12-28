using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoop : MonoBehaviour 
{
    public GameObject doneButton;

    private GameManager _manager;
    private List<GameObject> highLightObjects;
    private RaycastHit _touchBox;
    
    // variables for highlight and moving unit
    private Tile LastClickedUnit;
    private GameObject LastClickedUnitGameObject;
    
    private bool isHightlightOn = false;
    private bool moveUnit = false;
    private float startTime;
    private float duration = 2f;
    private Vector2 destionationLocation;
    private Vector2 startPosition;

    private RangeTiles rangeTiles;

	void Start () 
    {
        _manager = GameManager.Instance;
		_manager.SetupLevel();

        highLightObjects = new List<GameObject>();

        rangeTiles = new RangeTiles();
        rangeTiles.CreatePossibleRangeLocations();
	}
	
	void Update ()
    {
         // For testing the GetTileInRange() purposes.
        if (Input.GetKeyDown(KeyCode.A))
        {
            TestGetTilesInRange test = new TestGetTilesInRange();
            Dictionary<int, Dictionary<int, Tile>> testmovement = test.GetAllTilesWithinRange(new TileCoordinates(1, 1), 1);
            
            foreach (KeyValuePair<int, Dictionary<int, Tile>> item in testmovement)
            {
                foreach (KeyValuePair<int, Tile> val in item.Value)
                {
                    Debug.Log("ColumnId: " + item.Key + " | RowId: " + val.Key);
                }
            }
        }

        // If highlight is false show highlight from that player. If true then
        // we want to move if user selects on a highlight, else disable the highlights
        if (Input.GetMouseButtonDown(0))
        {
            if (!isHightlightOn)
            {
                ShowHighLight(_manager.currentPlayer);
            }
            else if (isHightlightOn)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out _touchBox))
                {
                    foreach (GameObject highlight in highLightObjects)
                    {
                        if (_touchBox.collider == highlight.collider)
                        {
                            moveUnit = true;
                            startTime = Time.time;
                            destionationLocation = highlight.transform.position;
                            startPosition = new Vector2(LastClickedUnit.transform.position.x, LastClickedUnit.transform.position.y);

                            //LastClickedUnit.ColumnId += 2;
                        }
                    }
                }

                foreach (GameObject highlights in highLightObjects)
                {
                    highlights.SetActive(false);
                }
                // recreate list, otherwise list gets filled with duplicates
                highLightObjects = new List<GameObject>();

                // disable current highlight
                isHightlightOn = false;

                // Call this method because we want to activate the highlight if user clicks on another unit
                ShowHighLight(_manager.currentPlayer);
            }
        }

        if (moveUnit)
        {
            LastClickedUnitGameObject.transform.position = Vector2.Lerp(startPosition, destionationLocation, (Time.time - startTime) / duration);

            // stop this after movement is done
        }

        ButtonClick.DoneButton(doneButton, _manager);
	}

    void ShowHighLight(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        foreach (UnitBase b in player.ownedUnits)
        {
            if (Physics.Raycast(ray, out _touchBox))
            {
                if (_touchBox.collider == b.unitGameObject.collider)
                {
                    if (!b.hasMoved)
                    {
                        LastClickedUnit = b.unitGameObject.tile;
                        LastClickedUnitGameObject = b.unitGameObject.gameObject;
                        ShowMovement(rangeTiles.possibleLocations[b.attackRange]);
                        isHightlightOn = true;
                    }
                }
            }
        }
    }

    // TODO: Need to get the total tiles in Y and X instead of <= 7 and <= 15
    void ShowMovement(PossibleLocations[] tiles)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].x + LastClickedUnit.ColumnId > 0 && tiles[i].y + LastClickedUnit.RowId > 0 && tiles[i].x + LastClickedUnit.ColumnId <= 15 && tiles[i].y + LastClickedUnit.RowId <= 7)
            {
                GameObject go = _manager.GetTile(new TileCoordinates(tiles[i].x + LastClickedUnit.ColumnId, tiles[i].y + LastClickedUnit.RowId)).transform.FindChild("highlight_move").gameObject;
                go.SetActive(true);

                // add to list so we can deactivate later
                highLightObjects.Add(go);
            }
        }
    }
}