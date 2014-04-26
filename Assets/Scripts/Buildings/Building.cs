using Assets.Scripts.Units;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Buildings
{
    public class Building
    {
        public BuildingGameObject BuildingGameObject { get; private set; }
        public int Income { get; private set; }
        public float CurrentCapturePoints { get; private set; }
        public bool CanProduce { get; set; }
        public float DamageToCapturingUnit { get; private set; }
        public float CapturePointsDecreasedBy { get; private set; }
        public int FowLineOfSightRange { get; set; }
        private Dictionary<UnitTypes, float> Modifiers { get; set; }

        public Building(BuildingGameObject game, int income, float capturePoints, bool canProduce,
         float damageToCapturingUnit, float CapturePointsDecreasedBy, int fowLos, int attackRange, float damage,
         Dictionary<UnitTypes, float> modifiers)
        {
            this.BuildingGameObject = game;
            this.Income = income;
            this.capturePoints = capturePoints;
            this.CurrentCapturePoints = 0;
            this.CanProduce = canProduce;
            this.DamageToCapturingUnit = damageToCapturingUnit;
            this.CapturePointsDecreasedBy = CapturePointsDecreasedBy;
            this.FowLineOfSightRange = fowLos;
            this.attackRange = attackRange;
            this.damage = damage;
            this.Modifiers = modifiers;
        }

        /// <summary>
        /// Returns the building modifier for the given UnitTypes.
        /// This method should not be called directly. The Unit class has the method GetGroundModifier() which does some checks and if need be calls this method.
        /// </summary>
        /// <param Name="type"></param>
        /// <returns></returns>
        public float GetBuildingModifier(UnitTypes type)
        {
            return Modifiers[type];
        }

        public float capturePoints { get; private set; }

        public void IncreaseCapturePointsBy(float increaseBy)
        {
            this.CurrentCapturePoints += increaseBy;
            this.CurrentCapturePoints = Mathf.Clamp(this.CurrentCapturePoints, 0f, this.capturePoints);
            BuildingGameObject.UpdateCapturePointsText();
        }

        public void DecreaseCapturePointsBy(float decreaseBy)
        {
            this.CurrentCapturePoints -= decreaseBy;
            this.CurrentCapturePoints = Mathf.Clamp(this.CurrentCapturePoints, 0f, this.capturePoints);
            BuildingGameObject.UpdateCapturePointsText();
        }

        public bool HasCaptured()
        {
            return CurrentCapturePoints >= capturePoints;
        }

        public void resetCurrentCapturePoints()
        {
            this.CurrentCapturePoints = 0f;
            BuildingGameObject.UpdateCapturePointsText();
        }

        public int attackRange { get; set; }
        public float damage { get; set; }
    }
}