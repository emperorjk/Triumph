using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DoneButton : MonoBehaviour
{
    private bool _needsFading = false;
    private bool _fadeIn = true;

    void Start()
    {
        Vector3 centerWorldSpace = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 5));
        transform.position = new Vector3(centerWorldSpace.x, centerWorldSpace.y + 1, centerWorldSpace.z);
        EventHandler.register<OnSwipeAction>(SwipeDoneButton);
        Color col = renderer.material.color;
        col.a = 0f;
        renderer.material.color = col;
    }

    void OnDestroy()
    {
        EventHandler.unregister<OnSwipeAction>(SwipeDoneButton);
    }

    void SwipeDoneButton(OnSwipeAction evt)
    {
        if(evt.SwipeUp)
        {
            GameManager.Instance.IsDoneButtonActive = !GameManager.Instance.IsDoneButtonActive;
            this.renderer.enabled = true;
            this.collider.enabled = true;
            _needsFading = true;
        }
    }

    private void Fade()
    {
        Color col = renderer.material.color;
        col.a = Mathf.Lerp(renderer.material.color.a, (_fadeIn ? 1 : 0), Time.deltaTime * 4.5f);

        if (!_fadeIn && col.a < 0.10f)
        {
            col.a = 0f;
            _needsFading = false;
            _fadeIn = true;
            this.renderer.enabled = GameManager.Instance.IsDoneButtonActive;
            this.collider.enabled = GameManager.Instance.IsDoneButtonActive;
        }
        else if (_fadeIn && col.a > 0.95f)
        {
            _fadeIn = false;
            _needsFading = false;
            col.a = 1f;
        }
        renderer.material.color = col;
    }

    void Update()
    {
        if(_needsFading) { Fade(); }

        if (Input.GetKeyDown(KeyCode.T) && !GameManager.Instance.IsDoneButtonActive && !_needsFading)
        {
            SwipeDoneButton(new OnSwipeAction(false, false, true, false));
        }

        if (Input.GetMouseButtonDown(0) && !_needsFading)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _touchBox;
            if (Physics.Raycast(ray, out _touchBox))
            {
                if (_touchBox.collider == this.collider)
                {
                    GameManager.Instance.EndTurn();
                    SwipeDoneButton(new OnSwipeAction(false, false, true, false));
                }
            }
        }
    }
}