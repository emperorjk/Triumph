using Assets.Scripts.Main;
using UnityEngine;

namespace Assets.Scripts.FactoryPattern.ProductionOverlayFactory
{
    public abstract class IProductionOverlay
    {
        public string DirToProductionOverlayFolder
        {
            get { return FileLocations.prefabUnitProduction; }
        }

        public abstract GameObject CreateProductionOverlay();
    }
}