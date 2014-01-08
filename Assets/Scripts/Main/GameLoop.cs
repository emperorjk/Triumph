using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Main gameloop
public class GameLoop : MonoBehaviour 
{
    public GameObject doneButton;

    private GameManager _manager;
    private RaycastHit _touchBox;
    private Movement _movement;
    
	void Start () 
    {
        _manager = GameManager.Instance;
		_manager.SetupLevel();

        _movement = new Movement();
	}

    void Update()
    {
        ActivateDoneButton();

        // If user clicks, check if highlight is on and show the highlights or check if user needs to move
        if (Input.GetMouseButtonDown(0) && !_manager.NeedMoving)
        {
            if (_manager.UnitCanAttack)
            {
                _movement.AttackCloseEnemy();
            }

            if (!_manager.IsHightlightOn)
            {
                _movement.ShowMovementUnit(_manager.CurrentPlayer);
            }
            else if (_manager.IsHightlightOn)
            {
                _movement.CollisionWithHighlight();
            }
        }

        if (_manager.NeedMoving)
        {
            _movement.Move();
        }
    }

    void ActivateDoneButton()
    {
        // for development KeyCode.Y testing DoneButton
        if (Input.touchCount == 2 || Input.GetKeyDown(KeyCode.Y))
        {
            if (!_manager.IsDoneButtonActive)
            {
                _manager.IsDoneButtonActive = true;
                doneButton.SetActive(true);
            }
            else
            {
                _manager.IsDoneButtonActive = false;
                doneButton.SetActive(false);
            }
        }

        // for development KeyCode.T next player
        if (Input.GetKeyDown(KeyCode.T))
        {
            _manager.NextPlayer();
        }
    }
}