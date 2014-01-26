using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class IProductionOverlay
{
    public string DirToProductionOverlayFolder { get { return FileLocations.prefabUnitProduction; } }
    public abstract GameObject CreateProductionOverlay();
}
