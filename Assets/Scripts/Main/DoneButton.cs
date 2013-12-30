using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DoneButton : MonoBehaviour
{
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