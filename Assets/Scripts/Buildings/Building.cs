using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building {

    public Building(BuildingGameObject game, int income, float capturePoints, bool canProduce, float damageToCapturingUnit, float CapturePointsDecreasedBy, int fowLos, int attackRange, float damage, Dictionary<UnitTypes, float> modifiers)
    {
        this.buildingGameObject = game;
        this.income = income;
        this.capturePoints = capturePoints;
        this.currentCapturePoints = 0;
        this.CanProduce = canProduce;
        this.DamageToCapturingUnit = damageToCapturingUnit;
        this.CapturePointsDecreasedBy = CapturePointsDecreasedBy;
        this.FowLineOfSightRange = fowLos;
        this.attackRange = attackRange;
        this.damage = damage;
        this.modifiers = modifiers;
    }
    public BuildingGameObject buildingGameObject { get; private set; }
    public int income { get; private set; }
    public float currentCapturePoints { get; private set; }
    public bool CanProduce { get; set; }
    public float DamageToCapturingUnit { get; private set; }
    public float CapturePointsDecreasedBy { get; private set; }
    public int FowLineOfSightRange { get; set; }
    private Dictionary<UnitTypes, float> modifiers { get; set; }
    /// <summary>
    /// Returns the building modifier for the given UnitTypes.
    /// This method should not be called directly. The Unit class has the method GetGroundModifier() which does some checks and if need be calls this method.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float GetBuildingModifier(UnitTypes type) { return modifiers[type]; }
    public float capturePoints { get; private set; }
    public void IncreaseCapturePointsBy(float increaseBy) 
    { 
        this.currentCapturePoints += increaseBy;
        this.currentCapturePoints = Mathf.Clamp(this.currentCapturePoints, 0f, this.capturePoints);
        buildingGameObject.UpdateCapturePointsText();
    }
    public void DecreaseCapturePointsBy(float decreaseBy) 
    { 
        this.currentCapturePoints -= decreaseBy;
        this.currentCapturePoints = Mathf.Clamp(this.currentCapturePoints, 0f, this.capturePoints);
        buildingGameObject.UpdateCapturePointsText();
    }
    public bool HasCaptured() { return currentCapturePoints >= capturePoints; }
    public void resetCurrentCapturePoints() 
    { 
        this.currentCapturePoints = 0f;
        buildingGameObject.UpdateCapturePointsText();
    }

    public int attackRange { get; set; }
    public float damage { get; set; }
}
