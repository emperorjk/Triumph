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
    /// <summary>
    /// Returns the sprite. Might change to texture or texture 2d if needed. For now this works.
    /// </summary>
    public abstract Sprite sprite { get; protected set; }
    public BuildingGameObject buildingGameObject { get; private set; }
    public int income { get; set; }
    public int currentCapturePoints { get; private set; }
    public int capturePoints { get; private set; }
    public void IncreaseCapturePointsBy(int increaseBy) 
    { 
        this.currentCapturePoints += increaseBy;
        if (this.currentCapturePoints > this.capturePoints) { this.currentCapturePoints = this.capturePoints; }
    }
    public void DecreaseCapturePointsBy(int decreaseBy) 
    { 
        this.currentCapturePoints -= decreaseBy;
        if (this.currentCapturePoints < 0) { this.currentCapturePoints = 0; }
    }
    public bool HasCaptured() { return currentCapturePoints >= capturePoints; }
    public void resetCurrentCapturePoints() { this.currentCapturePoints = 0; }
    public abstract BuildingTypes type { get; }
    /// <summary>
    /// Returns the production overlay. Don't know if this needs to be a Texture or Texture2D.
    /// </summary>
    public abstract Texture productionOverlay { get; }

    public void ShowProductionOverlay()
    {
        // spawn below the screen and move it up by x amount of units. Get the texture via the above property.
    }

}
