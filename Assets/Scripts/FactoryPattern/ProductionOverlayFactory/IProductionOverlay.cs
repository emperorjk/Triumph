using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class IProductionOverlay
{
    public string DirToUnitFolder { get { return "Prefabs/Units/UnitProduction/"; } }
    public abstract GameObject CreateProductionOverlay();
}
