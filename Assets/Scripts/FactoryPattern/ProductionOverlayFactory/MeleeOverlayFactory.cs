using UnityEngine;

namespace Assets.Scripts.FactoryPattern.ProductionOverlayFactory
{
    public class MeleeOverlayFactory : IProductionOverlay
    {
        public override GameObject CreateProductionOverlay()
        {
            GameObject obj = null;
            obj = Resources.Load<GameObject>(DirToProductionOverlayFolder + "ProductionMeleePrefab");
            return obj;
        }
    }
}
