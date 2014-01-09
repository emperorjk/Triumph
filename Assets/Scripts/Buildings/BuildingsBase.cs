using UnityEngine;
using System.Collections;

public abstract class BuildingsBase {

    protected BuildingsBase(BuildingGameObject game, int income, int capturePoints)
    {
        this.buildingGameObject = game;
        this.income = income;
        this.capturePoints = capturePoints;
        this.currentCapturePoints = 0;
    }
    public BuildingGameObject buildingGameObject { get; private set; }
    public int income { get; set; }
    public int currentCapturePoints { get; private set; }
    public int capturePoints { get; private set; }
    public void IncreaseCapturePointsBy(int increaseBy) 
    { 
        this.currentCapturePoints += increaseBy;
        if (this.currentCapturePoints >= this.capturePoints) { this.currentCapturePoints = this.capturePoints; }
        buildingGameObject.UpdateCapturePointsText();
    }
    public void DecreaseCapturePointsBy(int decreaseBy) 
    { 
        this.currentCapturePoints -= decreaseBy;
        if (this.currentCapturePoints <= 0) { this.currentCapturePoints = 0; }
        buildingGameObject.UpdateCapturePointsText();
    }
    public bool HasCaptured() { return currentCapturePoints >= capturePoints; }
    public void resetCurrentCapturePoints() 
    { 
        this.currentCapturePoints = 0;
        buildingGameObject.UpdateCapturePointsText();
    }
    public abstract BuildingTypes type { get; }
    public abstract bool CanProduce { get; }
    public abstract float DamageToCapturingUnit { get; }
    public abstract int FowLineOfSightRange { get; }
}
