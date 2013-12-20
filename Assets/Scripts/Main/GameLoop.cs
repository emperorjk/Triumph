using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    // key = range (1, 2, 3) value = possible locations for that key
    private Dictionary<int, PossibleLocations[]> rangeTiles;
    PossibleLocations[] tilesRangeOne;
    PossibleLocations[] tilesRangeTwo;
    PossibleLocations[] tilesRangeThree;
    PossibleLocations[] tilesRangeFour;

    // hardcoded tile, when we have units we need to get that tile
    private Tile player;

	void Start () 
    {
        _manager = GameManager.Instance;

        rangeTiles = new Dictionary<int, PossibleLocations[]>();
        tilesRangeOne = new PossibleLocations[4];
        tilesRangeTwo = new PossibleLocations[12];
        tilesRangeThree = new PossibleLocations[24];
        tilesRangeFour = new PossibleLocations[40];

        CreateRangeTiles();
	}
	
	void Update ()
    {
        // for testing purposes
        if (Input.GetKeyDown(KeyCode.D))
        {
            player = _manager.GetTile(new TileCoordinates(6, 3));
            player.transform.renderer.material.color = Color.yellow;            
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // create one range
            ShowMovement(rangeTiles[1], Color.grey);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            // create two range
            ShowMovement(rangeTiles[2], Color.cyan);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // create three range
            ShowMovement(rangeTiles[3], Color.black);
        }

        DoneButton();
	}

    // for testing I have added a color, will use overlay image later
    void ShowMovement(PossibleLocations[] tiles, Color col)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].x + player.ColumnId > 0 && tiles[i].y + player.RowId > 0)
                _manager.GetTile(new TileCoordinates(tiles[i].x + player.ColumnId, tiles[i].y + player.RowId)).renderer.material.color = col;
        }
    }

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

        for (int i = 0; i < 25; i++)
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