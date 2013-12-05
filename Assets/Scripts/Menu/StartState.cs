using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StartState : MenuStateBase
{
    private MenuManager _manager;
    public GameObject startButton;
    public GameObject guideButton;

    public StartState(MenuManager _manager)
    {
        this._manager = _manager;
        startButton = GameObject.Find("StartButton");
        guideButton = GameObject.Find("GuideButton");
    }

    public override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1.0f;
            Ray worldPos = Camera.main.ScreenPointToRay(mousePos);

            Debug.Log(worldPos);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.touchCount > 1)
        {
            _manager.ChangeState(MenuStates.LevelState);
            _manager.mainCamera.transform.position = _manager.menuPositions[1];
        }
        else if (Input.GetKeyDown(KeyCode.C) || Input.touchCount > 1)
        {
            _manager.ChangeState(MenuStates.GuideState);
            _manager.mainCamera.transform.position = _manager.menuPositions[2]; ;
        }
    }

    public override void OnActive()
    {

    }

    public override void OnInActive()
    {

    }
}
