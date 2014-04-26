using System.Collections.Generic;
using Assets.Scripts.Main;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Controls
{
    public class CameraControls : MonoBehaviour
    {
        // The margin used for the gamebar. So you can move just a little above the level in order to display the top row of tiles without the gamebar getting in the way.
        public float margin = 0f;
        // The speed at which the camera movement is done.
        private float speedCameraMovement = 5f;

        // Values used to determine the min and maximum x and y values of the game. (So where the tiles are placed.)
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;
        private int lastScreenWidth;
        private int lastScreenHeight;


        private void Start()
        {
            // Set all of the renderers that are childs of the camera to be on the GUI sorting layer.
            foreach (Renderer item in Camera.main.GetComponentsInChildren<Renderer>())
            {
                item.sortingLayerName = "GUI";
            }

            CalculateLevelArea();
            MoveCamera(new Vector2(0, 0));
        }

        private void Update()
        {
            CameraMovementInput();
        }

        private void CameraMovementInput()
        {
            if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
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
        /// <param Name="deltaposition">The amount the camera moves by.</param>
        private void MoveCamera(Vector2 deltaposition)
        {
            Camera cam = Camera.main;
            Vector3 distanceToMove = new Vector3(-deltaposition.x*speedCameraMovement*Time.deltaTime,
                -deltaposition.y*speedCameraMovement*Time.deltaTime, 0);
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

            GameManager manager = GameObject.Find("_Scripts").GetComponent<GameManager>();

            Dictionary<int, Tile> qq = manager.Tiles[manager.Tiles.Count];
            Tile first = manager.Tiles[1][1];
            Tile last = qq[qq.Count];

            Vector3 firstTilePosition = first.transform.position;
            Vector3 lastTilePosition = last.transform.position;

            Camera c = Camera.main;
            Vector3 UpperLeft = c.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f));
            Vector3 LowerRight = c.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f));
            float viewportWidth = LowerRight.x - UpperLeft.x;
            float viewportHeight = LowerRight.y - UpperLeft.y;

            minX = (firstTilePosition.x + viewportWidth/2.0f) - 1;
            maxX = (lastTilePosition.x - viewportWidth/2.0f) + 1;
            minY = (first.transform.parent.transform.position.y + viewportHeight/2.0f) + 1;
            maxY = (last.transform.parent.transform.position.y - viewportHeight/2.0f) - 1;
        }
    }
}