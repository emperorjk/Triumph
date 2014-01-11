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
    private RaycastHit _touchBox;
    private Highlight _highlight;


    // The margin used for the gamebar. So you can move just a little above the level in order to display the top row of tiles without the gamebar getting in the way.
    private float margin = 1.27f;
    // The speed at which the camera movement is done.
    private float speedCameraMovement = 4f;

    // Values used to determine the min and maximum x and y values of the game. (So where the tiles are placed.)
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

	void Start () 
    {
        _manager = GameManager.Instance;
		_manager.SetupLevel();

        _highlight = new Highlight();
        CalculateLevelArea();
        
        // Set all of the renderers that are childs of the camera to be on the GUI sorting layer.
        foreach (Renderer item in Camera.main.GetComponentsInChildren<Renderer>())
        {
            item.sortingLayerName = "GUI";
        }
	}

    void Update()
    {
        CameraMovementInput();

        GameManager.Instance.productionOverlayMain.OnUpdate();
        CheckDoneButton();

        _highlight.HandleHighlightInput();

    }

    private void CameraMovementInput()
    {
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
        Vector3 desiredLocation = new Vector3(-deltaposition.x * speedCameraMovement * Time.deltaTime, -deltaposition.y * speedCameraMovement * Time.deltaTime, 0);

        bool canMove = true;

        if(cam.transform.position.x + desiredLocation.x < minX)
        {
            canMove = false;
        }

        if (cam.transform.position.x + desiredLocation.x > maxX)
        {
            canMove = false;
        }

        if (cam.transform.position.y + desiredLocation.y > minY + margin)
        {
            canMove = false;
        }

        if (cam.transform.position.y + desiredLocation.y < maxY)
        {
            canMove = false;
        }
        
        if (canMove)
        {
            cam.transform.Translate(desiredLocation);
        }
    }


    /// <summary>
    /// Calculate the min and max x en y values of the game. These values are used to determine the values the camera position on the x and y position can go to.
    /// It takes into account the camera size and the maximum boundaries of the tiles in the level.
    /// </summary>
    private void CalculateLevelArea()
    {
        Dictionary<int, Tile> qq = _manager.tiles[_manager.tiles.Count];
        Tile first = _manager.tiles[1][1];
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