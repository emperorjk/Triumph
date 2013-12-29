using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        // If user clicks, check if highlight is on and show the highlights or check if user needs to move
        if (Input.GetMouseButtonDown(0))
        {
            if (!_manager.isHightlightOn)
            {
                _movement.ShowMovementHighLight(_manager.currentPlayer);
            }
            else if (_manager.isHightlightOn)
            {
                _movement.CollisionWithHightlight();
            }
        }

        ButtonClick.DoneButton(doneButton, _manager);
    }
}