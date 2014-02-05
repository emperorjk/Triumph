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
        Vector3 newPosition = new Vector3(centerWorldSpace.x, centerWorldSpace.y + 1, centerWorldSpace.z);
        transform.position = newPosition;
    }

    void Update()
    {
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
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}