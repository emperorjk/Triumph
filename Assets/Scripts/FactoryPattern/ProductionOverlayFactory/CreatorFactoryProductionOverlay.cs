using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CreatorFactoryProductionOverlay
{
    public static GameObject CreateProductionOverlay(BuildingTypes type)
    {
        if(type == BuildingTypes.Castle || type == BuildingTypes.Headquarters || type == BuildingTypes.TrainingZone)
        {
            throw new ArgumentException("Please provide a correct building type. The chosen building cannot produce.", "type");
        }
        GameObject obj = null;
        if (type == BuildingTypes.BarracksCavalry)
        {
            CavalryOverlayFactory fac = new CavalryOverlayFactory();
            obj = fac.CreateProductionOverlay();
        }
        else if (type == BuildingTypes.BarracksMelee)
        {
            MeleeOverlayFactory fac = new MeleeOverlayFactory();
            obj = fac.CreateProductionOverlay();
        }
        else if (type == BuildingTypes.BarracksRange)
        {
            RangeOverlayFactory fac = new RangeOverlayFactory();
            obj = fac.CreateProductionOverlay();
        }

        return (GameObject)GameObject.Instantiate(obj);
    }
}