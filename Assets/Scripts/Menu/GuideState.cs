using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GuideState : MenuStateBase
{
    private MenuManager _manager;
    private int activeImage = 0;
    private int previousImage = 0;

    public GuideState(MenuManager _manager)
    {
        this._manager = _manager;
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _manager.ChangeState(MenuStates.StartState);
            _manager.mainCamera.transform.position = _manager.menuPositions[0];
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeActiveImage(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeActiveImage(1);
        }
    }

    public override void OnActive()
    {

    }

    public override void OnInActive()
    {

    }

    private void ChangeActiveImage(int next)
    {
        previousImage = activeImage;

        activeImage += next;

        if (activeImage <= 0)
        {
            activeImage = 0;
        }
        else if (activeImage >= _manager.images.Count)
        {
            activeImage = _manager.images.Count - 1;
        }

        _manager.images[activeImage].gameObject.GetComponent<SpriteRenderer>().enabled = true;
        _manager.images[previousImage].gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}