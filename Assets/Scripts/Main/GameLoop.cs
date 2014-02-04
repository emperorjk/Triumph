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

	void Awake () 
    {
        _manager = GameManager.Instance;
        _manager.Init();
	}

    void Update()
    {
        CheckObjectsClick();
        CheckDoneButton();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.MuteAudio(true);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            AudioManager.MuteAudio(false);
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
            
            if(ouc.unit == null && ohc.highlight == null && !_manager.Movement.NeedsMoving || (_manager.Highlight.IsHighlightOn && ouc.unit != null))
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