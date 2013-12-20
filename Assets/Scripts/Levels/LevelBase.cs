using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Represents the basic Level attributes. Eventhough a level is created fully in the Unity environment a level still needs certain aspects like name and description and so forth.
/// </summary>
public abstract class LevelBase
{
    protected LevelBase(int lvlId, string lvlName)
    {
        this.LevelId = lvlId;
        this.LevelName = lvlName;
    }

    public int LevelId { get; set; }
    public string LevelName { get; set; }
    public string Description { get; set; }

    public abstract void SetupLevel();

}

