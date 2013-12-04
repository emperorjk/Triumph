using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StartState : MenuStateBase
{
    private MenuManager _manager;

    public StartState(MenuManager _manager)
    {
        this._manager = _manager;
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _manager.ChangeState(MenuStates.LevelState);
            _manager.mainCamera.transform.position = _manager.menuPositions[1];
        }
        else if (Input.GetKeyDown(KeyCode.C))
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
