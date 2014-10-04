using System;
using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.FactoryPattern.ProductionOverlayFactory
{

    public class CreatorFactoryProductionOverlay
    {
        public static GameObject CreateProductionOverlay(BuildingTypes type)
        {
            if (type == BuildingTypes.Castle || type == BuildingTypes.Headquarters || type == BuildingTypes.TrainingZone)
            {
                throw new ArgumentException(
                    "Please provide a correct building type. The chosen building cannot produce.", "type");
            }
            GameObject obj = null;
            IProductionOverlay po = null;

            if (type == BuildingTypes.BarracksCavalry) { po = new CavalryOverlayFactory(); }
            else if (type == BuildingTypes.BarracksMelee) { po = new MeleeOverlayFactory(); }
            else if (type == BuildingTypes.BarracksRange) { po = new RangeOverlayFactory(); }
            obj = po.CreateProductionOverlay();
            return (GameObject) GameObject.Instantiate(obj);
        }
    }
}