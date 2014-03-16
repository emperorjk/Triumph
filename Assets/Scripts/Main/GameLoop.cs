using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

// Main gameloop
public class GameLoop : MonoBehaviour 
{
    private GameManager _manager;
	
	void Awake () 
    {
        _manager = GameObject.Find("_Scripts").GetComponent<GameManager>();
	}

    bool test = false;
    void Update()
    {
        CheckObjectsClick();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.MuteAudio(test);
            test = !test;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _manager.LevelManager.LoadLevel(LevelsEnum.Menu);
        }
    }

    /// <summary>
    /// Check if there has been a click. Ifso then raycast and check if there has been clicked on a specific game object. Ifso fire an event with the click object as a parameter.
    /// </summary>
    private void CheckObjectsClick()
    {
        if (Input.GetMouseButtonDown(0) && !_manager.SwipeController.isSwipeHappening)
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
}