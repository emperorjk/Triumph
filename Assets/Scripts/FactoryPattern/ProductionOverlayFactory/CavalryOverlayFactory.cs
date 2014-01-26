using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class CavalryOverlayFactory : IProductionOverlay
{
    public override GameObject CreateProductionOverlay()
    {
        GameObject obj = null;
        obj = Resources.Load<GameObject>(DirToProductionOverlayFolder + "ProductionCavalryPrefab");
        return obj;
    }
}
