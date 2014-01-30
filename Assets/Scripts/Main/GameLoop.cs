using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

// Main gameloop
public class GameLoop : MonoBehaviour 
{
    public GameObject doneButton;
    private GameManager _manager;
    private Highlight _highlight;

    // The margin used for the gamebar. So you can move just a little above the level in order to display the top row of tiles without the gamebar getting in the way.
    public float margin = 1.7f;
    // The speed at which the camera movement is done.
    private float speedCameraMovement = 5f;

    // Values used to determine the min and maximum x and y values of the game. (So where the tiles are placed.)
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private int lastScreenWidth;
    private int lastScreenHeight;

	void Start () 
    {
        _manager = GameManager.Instance;
        _highlight = GameManager.Instance.Highlight;

        CalculateLevelArea();
        MoveCamera(new Vector2(0, 0));
        // Set all of the renderers that are childs of the camera to be on the GUI sorting layer.
        foreach (Renderer item in Camera.main.GetComponentsInChildren<Renderer>())
        {
            item.sortingLayerName = "GUI";
        }
	}

    void Update()
    {
        CheckObjectsClick();
        CameraMovementInput();

        GameManager.Instance.ProductionOverlayMain.OnUpdate();
        CheckDoneButton();

        _highlight.OnUpdate();
        _manager.AnimInfo.OnUpdate();
    }
    
    /// <summary>
    /// Check if there has been a click. Ifso then raycast and check if there has been clicked on a specific game object. Ifso fire an event with the click object as a parameter.
    /// </summary>
    private void CheckObjectsClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnUnitClick ouc = new OnUnitClick();
            OnBuildingClick obc = new OnBuildingClick();
            OnHighlightClick ohc = new OnHighlightClick();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                foreach (UnitBase unit in _manager.CurrentPlayer.ownedUnits)
                {
                    if (unit.unitGameObject.collider == hit.collider)
                    {
                        ouc.unit = unit.unitGameObject;
                        break;
                    }
                }   
                foreach (BuildingsBase building in _manager.CurrentPlayer.ownedBuildings)
                {
                    if (building.buildingGameObject.collider == hit.collider)
                    {
                        obc.building = building.buildingGameObject;
                        break;
                    }
                }
                foreach (HighlightObject highlight in _highlight.HighlightObjects)
                {
                    if (highlight.collider == hit.collider)
                    {
                        ohc.highlight = highlight;
                        break;
                    }
                }
            }
            
            if(ouc.unit == null && ohc.highlight == null && !_manager.Movement.needsMoving || (_manager.Highlight.IsHighlightOn && ouc.unit != null))
            {
                _manager.Highlight.ClearNewHighlights();
            }
            EventHandler.dispatch(ouc);
            EventHandler.dispatch(obc);
            EventHandler.dispatch(ohc);
        }
    }


    private void CameraMovementInput()
    {
        if(lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
        {
            CalculateLevelArea();
            MoveCamera(new Vector2(0, 0));
        }

        // This first bit is just used for the keyboard controls.
        float xx = 0;
        float yy = 0;
        // this speed is used for the keyboard.
        float speed = 4;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            yy -= speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            yy += speed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xx += speed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            xx -= speed;
        }

        if (xx != 0 || yy != 0)
        {
            MoveCamera(new Vector2(xx, yy));
        }

        // If there is only 1 finger touching the screen.
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            // Only try and update the camera when the finger is moving across the screen.
            if (t.phase == TouchPhase.Moved)
            {
                MoveCamera(t.deltaPosition);
            }
        }
    }

    /// <summary>
    /// Moves the camera according to the given Vector2 deltaposition.
    /// </summary>
    /// <param name="deltaposition">The amount the camera moves by.</param>
    private void MoveCamera(Vector2 deltaposition)
    {
        Camera cam = Camera.main;
        Vector3 distanceToMove = new Vector3(-deltaposition.x * speedCameraMovement * Time.deltaTime, -deltaposition.y * speedCameraMovement * Time.deltaTime, 0);
        Vector3 positionMovingTo = cam.transform.position + distanceToMove;
        positionMovingTo.x = Mathf.Clamp(positionMovingTo.x, minX, maxX);
        // The minY and maxY need to be switched around. Since y axis in the level is always negative.
        positionMovingTo.y = Mathf.Clamp(positionMovingTo.y, maxY, minY + margin);
        cam.transform.position = positionMovingTo;
    }


    /// <summary>
    /// Calculate the min and max x en y values of the game. These values are used to determine the values the camera position on the x and y position can go to.
    /// It takes into account the camera size and the maximum boundaries of the tiles in the level.
    /// </summary>
    private void CalculateLevelArea()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        Dictionary<int, Tile> qq = _manager.Tiles[_manager.Tiles.Count];
        Tile first = _manager.Tiles[1][1];
        Tile last = qq[qq.Count];

        Vector3 firstTilePosition = first.transform.position;
        Vector3 lastTilePosition = last.transform.position;

        Camera c = Camera.main;
        Vector3 UpperLeft = c.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f));
        Vector3 LowerRight = c.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f));
        float viewportWidth = LowerRight.x - UpperLeft.x;
        float viewportHeight = LowerRight.y - UpperLeft.y;

        minX = (firstTilePosition.x + viewportWidth / 2.0f) - 1;
        maxX = (lastTilePosition.x - viewportWidth / 2.0f) + 1;
        minY = (first.transform.parent.transform.position.y + viewportHeight / 2.0f) + 1;
        maxY = (last.transform.parent.transform.position.y - viewportHeight / 2.0f) - 1;
    }

    void CheckDoneButton()
    {
        // for development KeyCode.Y testing DoneButton
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && Input.touches[0].tapCount == 2 || Input.GetKeyDown(KeyCode.Y))
        {
            _manager.IsDoneButtonActive = !_manager.IsDoneButtonActive;
            doneButton.SetActive(_manager.IsDoneButtonActive);
        }

        // for development KeyCode.T next player
        if (Input.GetKeyDown(KeyCode.T))
        {
            _manager.NextPlayer();
        }
    }
}