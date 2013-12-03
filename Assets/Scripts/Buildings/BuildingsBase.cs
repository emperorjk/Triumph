using UnityEngine;
using System.Collections;

public abstract class BuildingsBase {

    protected BuildingsBase(int income, int capturePoints)
    {
        this.income = income;
        this.capturePoints = capturePoints;
    }

    public int income { get; set; }
    public int capturePoints { get; set; }
    public abstract BuildingTypes type { get; }
}
