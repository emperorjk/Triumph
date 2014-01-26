﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class IBuildingGameObject
{
    public string DirToBuildingFolder { get { return FileLocations.prefabBuildings; } }
    public abstract GameObject CreateBuilding(PlayerIndex index);
}
