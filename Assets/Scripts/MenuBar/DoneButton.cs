using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DoneButton : MonoBehaviour
{

    void Start()
    {
        Vector3 centerWorldSpace = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 5));
        transform.position = new Vector3(centerWorldSpace.x, centerWorldSpace.y + 1, centerWorldSpace.z);
        EventHandler.register<OnSwipeAction>(SwipeDoneButton);
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
            this.renderer.enabled = GameManager.Instance.IsDoneButtonActive;
            this.collider.enabled = GameManager.Instance.IsDoneButtonActive;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && !GameManager.Instance.IsDoneButtonActive)
        {
            // Fake swipe in order to have the code in 1 place.
            SwipeDoneButton(new OnSwipeAction(false, false, true, false));
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _touchBox;
            if (Physics.Raycast(ray, out _touchBox))
            {
                if (_touchBox.collider == this.collider)
                {
                    GameManager.Instance.EndTurn();
                    GameManager.Instance.IsDoneButtonActive = false;
                    this.renderer.enabled = GameManager.Instance.IsDoneButtonActive;
                    this.collider.enabled = GameManager.Instance.IsDoneButtonActive;
                }
            }
        }
    }
}