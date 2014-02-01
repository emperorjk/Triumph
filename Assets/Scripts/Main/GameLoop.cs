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
    private CameraControls _cameraControls;

    private List<IGameloop> _gameloopObjects;

	void Awake () 
    {
        _gameloopObjects = new List<IGameloop>();
        _manager = GameManager.Instance;
        _cameraControls = new CameraControls();


        // Add all of the classes which implements the IGameloop interface to the list.
        _gameloopObjects.Add(_cameraControls);
        _gameloopObjects.Add(_manager.ProductionOverlayMain);
        _gameloopObjects.Add(_manager.Movement);
        _gameloopObjects.Add(_manager.AnimInfo);
        
        foreach (IGameloop item in _gameloopObjects)
        {
            item.OnAwake();
        }
	}

    void Start()
    {
        foreach (IGameloop item in _gameloopObjects)
        {
            item.OnStart();
        }
    }

    void Update()
    {
        CheckObjectsClick();
        CheckDoneButton();

        foreach (IGameloop item in _gameloopObjects)
        {
            item.OnUpdate();
        }
    }

    void FixedUpdate()
    {
        foreach (IGameloop item in _gameloopObjects)
        {
            item.OnFixedUpdate();
        }
    }
    
    void LateUpdate()
    {
        foreach (IGameloop item in _gameloopObjects)
        {
            item.OnLateUpdate();
        }
    }

    void OnDestroy()
    {
        foreach (IGameloop item in _gameloopObjects)
        {
            item.OnDestroy();
        }
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
                    if (unit.UnitGameObject.collider == hit.collider)
                    {
                        ouc.unit = unit.UnitGameObject;
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
                foreach (HighlightObject highlight in _manager.Highlight.HighlightObjects)
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
                _manager.Highlight.ClearHighlights();
            }
            EventHandler.dispatch(ouc);
            EventHandler.dispatch(obc);
            EventHandler.dispatch(ohc);
        }
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