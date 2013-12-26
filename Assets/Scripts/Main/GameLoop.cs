using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Replace PossibleLocations with TileCoordinates? Has the same content and its used to get tile etc.
// Maybe, TileCoordinates is the coordinate of a Tile, but PossibleLocations is the locations user
// can move or attack relative to their positions.
public struct PossibleLocations 
{
    public int x { get; private set; }
    public int y { get; private set; }
    public PossibleLocations(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class GameLoop : MonoBehaviour {
    public GameObject doneButton;

    private GameManager _manager;
    private float _tapTimer = 0.5f;
    private float _resetTimer = 0.5f;
    private float _tapCount = 0;
    private RaycastHit _touchBox;
    private GameObject _doneButtonGameObject;

    // key = range (1, 2, 3, 4) value = possible locations for that key
    private Dictionary<int, PossibleLocations[]> rangeTiles;
    PossibleLocations[] tilesRangeOne;
    PossibleLocations[] tilesRangeTwo;
    PossibleLocations[] tilesRangeThree;
    PossibleLocations[] tilesRangeFour;

    // Last clicked unit to show range / attack
    private Tile LastClickedUnit;
    private GameObject LastClickedUnitGameObject;
    private bool isHightlightOn = false;
    private List<GameObject> highLightObjects;
    private bool moveUnit = false;
    private Vector2 destionationLoc;
    private Vector2 t;
    private float startTime;
    private float duration = 2f;

	void Start () 
    {
        _manager = GameManager.Instance;
		_manager.SetupLevel();
        startTime = Time.time;

        highLightObjects = new List<GameObject>();
        rangeTiles = new Dictionary<int, PossibleLocations[]>();
        tilesRangeOne = new PossibleLocations[4];
        tilesRangeTwo = new PossibleLocations[12];
        tilesRangeThree = new PossibleLocations[24];
        tilesRangeFour = new PossibleLocations[40];

        CreateRangeTiles();
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
                    foreach (GameObject go in highLightObjects)
                    {
                        if (_touchBox.collider == go.collider)
                        {
                            moveUnit = true;
                            startTime = Time.time;
                            destionationLoc = go.transform.position;
                            t = new Vector2(LastClickedUnit.transform.position.x, LastClickedUnit.transform.position.y);
                        }
                    }
                }

                foreach (GameObject go in highLightObjects)
                {
                    go.SetActive(false);
                }
                // remake list, otherwise list gets filled with duplicates
                highLightObjects = new List<GameObject>();

                // disable current highlight
                isHightlightOn = false;

                // Call this method because we want to activate the highlight if user clicks on another unit
                ShowHighLight(_manager.currentPlayer);
            }
        }

        if (moveUnit)
        {          
            // Create Vector2, this is the position of the clicked unit
            // Destination Vector2 is destionationLoc
            // We need to lerp from the one to the other
            LastClickedUnitGameObject.transform.position = Vector2.Lerp(t, destionationLoc, (Time.time - startTime) / duration);
            Debug.Log(LastClickedUnitGameObject.transform.position);
        }

        DoneButton();
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
                        ShowMovement(rangeTiles[b.attackRange]);
                        isHightlightOn = true;
                    }
                }
            }
        }
    }

    void ShowMovement(PossibleLocations[] tiles)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].x + LastClickedUnit.ColumnId > 0 && tiles[i].y + LastClickedUnit.RowId > 0)
            {
                GameObject go = _manager.GetTile(new TileCoordinates(tiles[i].x + LastClickedUnit.ColumnId, tiles[i].y + LastClickedUnit.RowId)).transform.FindChild("Highlight").gameObject;
                go.SetActive(true);

                // add to list so we can deactivate later
                highLightObjects.Add(go);
            }
        }
    }

    // DoneButton click method
    #region DoneButton
    void DoneButton()
    {
        if (!_manager.isDoneButtonActive)
        {
            // start timer when user has clicked once
            if (_tapCount == 1)
            {
                _tapTimer -= Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (_tapTimer >= 0)
                {
                    _tapCount++;

                    if (_tapCount >= 2)
                    {
                        doneButton.SetActive(true);
                        _manager.isDoneButtonActive = true;
                        _tapCount = 0;
                        _tapTimer = _resetTimer;
                    }
                }
                else
                {
                    _tapTimer = _resetTimer;
                    _tapCount = 0;
                }
            }
        }
        else if (_manager.isDoneButtonActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out _touchBox))
                {
                    if (_touchBox.collider == doneButton.collider)
                    {
                        doneButton.SetActive(false);
                        _manager.isDoneButtonActive = false;
                        _manager.NextPlayer();
                    }
                    else
                    {
                        doneButton.SetActive(false);
                        _manager.isDoneButtonActive = false;
                    }
                }
            }
        }
    }
    #endregion

    // Create range tiles only once. With these locations we can create the movement and attack range. See ShowMovement method.
    #region CreateRangeTiles
    private void CreateRangeTiles()
    {
        tilesRangeOne[0] = new PossibleLocations(0, 1);
        tilesRangeOne[1] = new PossibleLocations(0, -1);
        tilesRangeOne[2] = new PossibleLocations(1, 0);
        tilesRangeOne[3] = new PossibleLocations(-1, 0);

        for (int i = 0; i < 4; i++)
        {
            tilesRangeTwo[i] = tilesRangeOne[i];
        }

        tilesRangeTwo[4] = new PossibleLocations(-1, -1);
        tilesRangeTwo[5] = new PossibleLocations(1, -1);
        tilesRangeTwo[6] = new PossibleLocations(-1, 1);
        tilesRangeTwo[7] = new PossibleLocations(1, 1);
        tilesRangeTwo[8] = new PossibleLocations(2, 0);
        tilesRangeTwo[9] = new PossibleLocations(-2, 0);
        tilesRangeTwo[10] = new PossibleLocations(0, 2);
        tilesRangeTwo[11] = new PossibleLocations(0, -2);

        for (int i = 0; i < 12; i++ )
        {
            tilesRangeThree[i] = tilesRangeTwo[i];
        }

        tilesRangeThree[12] = new PossibleLocations(-2, 1);
        tilesRangeThree[13] = new PossibleLocations(2, 1);
        tilesRangeThree[14] = new PossibleLocations(-1, 2);
        tilesRangeThree[15] = new PossibleLocations(1, 2);
        tilesRangeThree[16] = new PossibleLocations(3, 0);
        tilesRangeThree[17] = new PossibleLocations(-3, 0);
        tilesRangeThree[18] = new PossibleLocations(0, 3);
        tilesRangeThree[19] = new PossibleLocations(0, -3);
        tilesRangeThree[20] = new PossibleLocations(2, -1);
        tilesRangeThree[21] = new PossibleLocations(1, -2);
        tilesRangeThree[22] = new PossibleLocations(-2, -1);
        tilesRangeThree[23] = new PossibleLocations(-1, -2);
        // changed i < 25 to i < 24 because it gave a out of index exception. perhaps using Count?
        for (int i = 0; i < 24; i++)
        {
            tilesRangeFour[i] = tilesRangeThree[i];
        }

        tilesRangeFour[12] = new PossibleLocations(-4, 0);
        tilesRangeFour[12] = new PossibleLocations(4, 0);
        tilesRangeFour[12] = new PossibleLocations(0, -4);
        tilesRangeFour[12] = new PossibleLocations(0, 4);
        tilesRangeFour[12] = new PossibleLocations(1, -3);
        tilesRangeFour[12] = new PossibleLocations(1, 3);
        tilesRangeFour[12] = new PossibleLocations(-1, -3);
        tilesRangeFour[12] = new PossibleLocations(-1, 3);
        tilesRangeFour[12] = new PossibleLocations(-2, -2);
        tilesRangeFour[12] = new PossibleLocations(-2, 2);
        tilesRangeFour[12] = new PossibleLocations(2, -2);
        tilesRangeFour[12] = new PossibleLocations(2, 2);
        tilesRangeFour[12] = new PossibleLocations(-3, -1);
        tilesRangeFour[12] = new PossibleLocations(-3, 1);
        tilesRangeFour[12] = new PossibleLocations(3, -1);
        tilesRangeFour[12] = new PossibleLocations(3, 1);

        rangeTiles.Add(1, tilesRangeOne);
        rangeTiles.Add(2, tilesRangeTwo);
        rangeTiles.Add(3, tilesRangeThree);
        rangeTiles.Add(4, tilesRangeFour);
    }
    #endregion
}