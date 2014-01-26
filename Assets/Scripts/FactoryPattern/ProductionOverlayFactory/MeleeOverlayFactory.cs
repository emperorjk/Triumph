using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class MeleeOverlayFactory : IProductionOverlay
{
    public override GameObject CreateProductionOverlay()
    {
        GameObject obj = null;
        obj = Resources.Load<GameObject>(DirToProductionOverlayFolder + "ProductionMeleePrefab");
        return obj;
    }
}
