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

    // Min swipe distance downwards.
    private float minSwipeDistance = 50f;
    // The difference the fingers can go to the left or right.
    private float swipeXVariance = 30f;
    private bool isSwipeHappening = false;
    private Vector2 SwipingVector;
	
	void Awake () 
    {
        _manager = GameManager.Instance;
        _manager.Init();
	}
    bool test = false;
    void Update()
    {
        DoneButton();
        CheckObjectsClick();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.MuteAudio(test);
            test = !test;
        }
    }

    void OnDestroy()
    {
        _manager.OnGameloopDestroy();
    }

    /// <summary>
    /// Check if there has been a click. Ifso then raycast and check if there has been clicked on a specific game object. Ifso fire an event with the click object as a parameter.
    /// </summary>
    private void CheckObjectsClick()
    {
        if (Input.GetMouseButtonDown(0) && !isSwipeHappening)
        {
            OnUnitClick ouc = new OnUnitClick();
            OnBuildingClick obc = new OnBuildingClick();
            OnHighlightClick ohc = new OnHighlightClick();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                foreach (Unit unit in _manager.CurrentPlayer.ownedUnits)
                {
                    if (unit.UnitGameObject.collider == hit.collider)
                    {
                        ouc.unit = unit.UnitGameObject;
                        break;
                    }
                }   
                foreach (Building building in _manager.CurrentPlayer.ownedBuildings)
                {
                    if (building.buildingGameObject.collider == hit.collider)
                    {
                        obc.building = building.buildingGameObject;
                        break;
                    }
                }
                foreach (HighlightObject highlight in _manager.Highlight.HighlightObjects)
                {
                    if (highlight.collider == hit.collider)
                    {
                        ohc.highlight = highlight;
                        break;
                    }
                }
            }
            
            if(ouc.unit == null && ohc.highlight == null && !_manager.Movement.NeedsMoving || (_manager.Highlight.IsHighlightOn && ouc.unit != null))
            {
                _manager.Highlight.ClearHighlights();
            }
            EventHandler.dispatch(ouc);
            EventHandler.dispatch(obc);
            EventHandler.dispatch(ohc);
        }
    }

    void DoneButton()
    {
        if (Input.touchCount == 2)
        {
            foreach (Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Began && !isSwipeHappening)
                {
                    SwipingVector = touch.position;
                    isSwipeHappening = true;
                }
                else if(touch.phase == TouchPhase.Moved && isSwipeHappening)
                {
                    Vector2 delta = touch.position - SwipingVector;
                    if(delta.magnitude > minSwipeDistance && delta.y < 0 && Mathf.Abs(delta.x) < swipeXVariance)
                    {
                        isSwipeHappening = false;
                        _manager.IsDoneButtonActive = !_manager.IsDoneButtonActive;
                        doneButton.SetActive(_manager.IsDoneButtonActive);
                    }
                }
                else if (touch.phase == TouchPhase.Ended && isSwipeHappening)
                {
                    SwipingVector = Vector2.zero;
                    isSwipeHappening = false;
                }
            }
        }

        // for development KeyCode.T next player
        if (Input.GetKeyDown(KeyCode.T))
        {
            _manager.EndTurn();
        }
    }
}