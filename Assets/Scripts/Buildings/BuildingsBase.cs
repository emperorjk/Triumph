using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingsBase {

    public BuildingsBase(BuildingGameObject game, int income, int capturePoints, bool canProduce, float damageToCapturingUnit, int fowLos, int attackRange, float damage, Dictionary<UnitTypes, float> modifiers)
    {
        this.buildingGameObject = game;
        this.income = income;
        this.capturePoints = capturePoints;
        this.currentCapturePoints = 0;
        this.CanProduce = canProduce;
        this.DamageToCapturingUnit = damageToCapturingUnit;
        this.FowLineOfSightRange = fowLos;
        this.attackRange = attackRange;
        this.damage = damage;
        this.modifiers = modifiers;
    }
    public BuildingGameObject buildingGameObject { get; private set; }
    public int income { get; private set; }
    public int currentCapturePoints { get; private set; }
    public bool CanProduce { get; set; }
    public float DamageToCapturingUnit { get; set; }
    public int FowLineOfSightRange { get; set; }
    private Dictionary<UnitTypes, float> modifiers { get; set; }
    public float GetModifier(UnitTypes type) { return modifiers[type]; }
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

    public int attackRange { get; set; }
    public float damage { get; set; }
}
