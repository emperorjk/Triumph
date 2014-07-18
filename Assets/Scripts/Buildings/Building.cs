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
        public float CapturePoints { get; private set; }
        public bool CanProduce { get; set; }
        public float DamageToCapturingUnit { get; private set; }
        public float CapturePointsDecreasedBy { get; private set; }
        public int FowLineOfSightRange { get; set; }
        public int AttackRange { get; private set; }
        public float Damage { get; private set; }
        public Dictionary<UnitTypes, float> Modifiers { get; private set; }

        public Building(BuildingGameObject game, int income, float capturePoints, bool canProduce,
         float damageToCapturingUnit, float capturePointsDecreasedBy, int fowLos, int attackRange, float damage,
         Dictionary<UnitTypes, float> modifiers)
        {
            BuildingGameObject = game;
            Income = income;
            CapturePoints = capturePoints;
            CurrentCapturePoints = 0;
            CanProduce = canProduce;
            DamageToCapturingUnit = damageToCapturingUnit;
            CapturePointsDecreasedBy = capturePointsDecreasedBy;
            FowLineOfSightRange = fowLos;
            AttackRange = attackRange;
            Damage = damage;
            Modifiers = modifiers;
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


        public void IncreaseCapturePointsBy(float increaseBy)
        {
            CurrentCapturePoints += increaseBy;
            CurrentCapturePoints = Mathf.Clamp(CurrentCapturePoints, 0f, CapturePoints);
            BuildingGameObject.UpdateCapturePointsText();
        }

        public void DecreaseCapturePointsBy(float decreaseBy)
        {
            CurrentCapturePoints -= decreaseBy;
            CurrentCapturePoints = Mathf.Clamp(CurrentCapturePoints, 0f, CapturePoints);
            BuildingGameObject.UpdateCapturePointsText();
        }

        public bool HasCaptured()
        {
            return CurrentCapturePoints >= CapturePoints;
        }

        public void ResetCurrentCapturePoints()
        {
            CurrentCapturePoints = 0f;
            BuildingGameObject.UpdateCapturePointsText();
        }
    }
}