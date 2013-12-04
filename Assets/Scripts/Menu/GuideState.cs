using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GuideState : MenuStateBase
{
    private MenuManager _manager;
    private int activeImage = 0;

    public GuideState(MenuManager _manager)
    {
        this._manager = _manager;
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (activeImage == 0)
            {
                _manager.ChangeState(MenuStates.StartState);
                _manager.mainCamera.transform.position = _manager.menuPositions[0];
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (activeImage == 0)
            {
                ActiveImage(true);
            }
            else if (activeImage == 1)
            {
                ActiveImage(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (activeImage == 1)
            {
                ActiveImage(false);
            }
            else if (activeImage == 2)
            {
                ActiveImage(false);
            }
        }
    }

    public override void OnActive()
    {

    }

    public override void OnInActive()
    {

    }

    private void ActiveImage(bool next)
    {
        _manager.images[activeImage].gameObject.GetComponent<SpriteRenderer>().enabled = false;

        if (next)
        {
            _manager.images[activeImage + 1].gameObject.GetComponent<SpriteRenderer>().enabled = true;
            activeImage++;
        }
        else 
        {
            _manager.images[activeImage - 1].gameObject.GetComponent<SpriteRenderer>().enabled = true;
            activeImage--;
        }
    }
}