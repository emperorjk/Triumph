using UnityEngine;

namespace Assets.Scripts.FactoryPattern.ProductionOverlayFactory
{
    public class RangeOverlayFactory : IProductionOverlay
    {
        public override GameObject CreateProductionOverlay()
        {
            GameObject obj = null;
            obj = Resources.Load<GameObject>(DirToProductionOverlayFolder + "ProductionRangePrefab");
            return obj;
        }
    }
}
