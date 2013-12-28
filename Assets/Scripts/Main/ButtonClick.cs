using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// In future maybe multiple click methods
public static class ButtonClick
{
    // variables for done button
    private static float _tapTimer = 0.4f;
    private static float _resetTimer = 0.4f;
    private static float _tapCount = 0;
    private static RaycastHit _touchBox;

    // DoneButton click method
    public static void DoneButton(GameObject doneButton, GameManager _manager)
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