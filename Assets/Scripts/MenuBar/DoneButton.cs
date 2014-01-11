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

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            GameManager.Instance.NextPlayer();
            GameManager.Instance.IsDoneButtonActive = false;
            this.gameObject.SetActive(false);
        }
    } 
}