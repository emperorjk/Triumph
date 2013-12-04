using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelState : MenuStateBase
{
    private MenuManager _manager;

    public LevelState(MenuManager _manager)
    {
        this._manager = _manager;
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Start het spel");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            _manager.ChangeState(MenuStates.StartState);
            _manager.mainCamera.transform.position = _manager.menuPositions[0];
        }
    }

    public override void OnActive()
    {

    }

    public override void OnInActive()
    {

    }
}