using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Testing : MonoBehaviour 
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            LevelSerializer.SerializeLevelToFile("TheGame");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelSerializer.LoadSavedLevelFromFile("TheGame");
        }
    }
}
