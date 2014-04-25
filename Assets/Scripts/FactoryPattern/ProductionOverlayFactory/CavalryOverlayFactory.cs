using UnityEngine;

namespace Assets.Scripts.FactoryPattern.ProductionOverlayFactory
{
    public class CavalryOverlayFactory : IProductionOverlay
    {
        public override GameObject CreateProductionOverlay()
        {
            GameObject obj = null;
            obj = Resources.Load<GameObject>(DirToProductionOverlayFolder + "ProductionCavalryPrefab");
            return obj;
        }
    }
}