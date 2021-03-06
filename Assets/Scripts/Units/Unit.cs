﻿using Assets.Scripts.Audio;
using Assets.Scripts.DayNight;
using Assets.Scripts.Main;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Units
{
    public class Unit
    {
        public Unit(UnitGameObject game, bool isHero, int attackRange, int moveRange, bool canAttackAfterMove,
            float maxHealth,
            float damage, int cost, int fowLos, float baseLoot, Dictionary<UnitTypes, float> modifiers)
        {
            UnitGameObject = game;
            IsHero = isHero;
            HasMoved = false;
            HasAttacked = false;
            AttackRange = attackRange;
            MoveRange = moveRange;
            CanAttackAfterMove = canAttackAfterMove;
            CurrentHealth = maxHealth;
            MaxHealth = maxHealth;
            Damage = damage;
            Cost = cost;
            FowLineOfSightRange = fowLos;
            BaseLoot = baseLoot;
            CurrentLoot = baseLoot;
            Modifiers = modifiers;
        }

        public UnitGameObject UnitGameObject { get; private set; }
        public bool IsHero { get; private set; }

        public bool HasMoved { get; set; }

        public bool HasAttacked { get; set; }

        public int AttackRange
        {
            get
            {
                return GameObject.Find("_Scripts").GetComponent<DayStateController>().CurrentDayState ==
                       DayStates.Night
                    ? 1
                    : _AttackRange;
            }
            private set { _AttackRange = value; }
        }

        private int _AttackRange;

        public int MoveRange
        {
            get
            {
                return GameObject.Find("_Scripts").GetComponent<DayStateController>().CurrentDayState ==
                       DayStates.Night
                    ? Mathf.Clamp(_MoveRange - 1, 1, int.MaxValue)
                    : _MoveRange;
            }
            private set { _MoveRange = value; }
        }

        private int _MoveRange;

        public int GetAttackMoveRange
        {
            get { return CanAttackAfterMove ? AttackRange + MoveRange : AttackRange; }
        }

        public bool CanAttackAfterMove { get; set; }
        public float CurrentLoot { get; private set; }

        public float DeliverLoot()
        {
            float amount = 0;

            if (CurrentLoot > BaseLoot)
            {
                amount = CurrentLoot - BaseLoot;
                CurrentLoot = BaseLoot;
            }
            return amount;
        }

        public void AddLoot(float loot)
        {
            CurrentLoot += loot;
        }

        public float BaseLoot { get; private set; }

        public int FowLineOfSightRange { get; set; }

        public void PlaySound(UnitSoundType soundType)
        {
            //GameObject.Find("_Scripts").GetComponent<GameManager>().UnitSounds.PlaySound(UnitGameObject.type, soundType);
        }

        public void DecreaseHealth(float damage)
        {
            CurrentHealth -= damage;
            UnitGameObject.UpdateHealthText();
        }

        public void IncreaseHealth(float recovery)
        {
            CurrentHealth += recovery;
            // If we later have some sort of healing make sure that it cannot go over its initial full health.
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            //if (this.currentHealth >= this.health) { this.currentHealth = this.health; }
            UnitGameObject.UpdateHealthText();
        }

        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }

        public void OnDeath()
        {
            GameObject loot =
                ((GameObject) GameObject.Instantiate(Resources.Load<GameObject>(FileLocations.lootPrefab)));
            UnitGameObject.Tile.Loot = loot.GetComponent<Loot>();
            UnitGameObject.Tile.Loot.SetLoot(CurrentLoot);
            loot.GetComponent<Loot>().Tile = UnitGameObject.Tile;
            loot.transform.position = UnitGameObject.Tile.gameObject.transform.position;

            UnitGameObject.DestroyUnit();
        }

        public void UpdateUnitColor()
        {
            UnitGameObject.gameObject.renderer.material.color = HasMoved && HasAttacked ? Color.gray : Color.white;
        }

        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public float Damage { get; private set; }
        public int Cost { get; private set; }

        public float GetStrength()
        {
            return CurrentHealth/10f;
        }

        private Dictionary<UnitTypes, float> Modifiers { get; set; }

        /// <summary>
        /// Returns how good this unit is against the UnitTypes that is given as a parameter.
        /// </summary>
        /// <param Name="enemyUnit"></param>
        /// <returns></returns>
        public float GetUnitModifier(UnitTypes enemyUnit)
        {
            return Modifiers[enemyUnit];
        }

        /// <summary>
        /// Returns the building or Tile modifier the unit is standing on. If there is a building return the building modifier, otherwise return the environment modifier.
        /// </summary>
        /// <returns></returns>
        public float GetGroundModifier()
        {
            if (UnitGameObject.Tile.HasBuilding())
            {
                // Removed this because it can help the player. If normally the unit is given a 0.5 modifier when it owns the building, it will now receive a 1.0f modifier
                // which increases the Damage done and thus neglecting the fact that a knight is bad on a castle for example.
                //if(UnitGameObject.Tile.BuildingGameObject.Index != UnitGameObject.Index)
                // {
                //    return 1f;
                // }
                return UnitGameObject.Tile.buildingGameObject.BuildingGame.GetBuildingModifier(UnitGameObject.type);
            }
            return
                UnitGameObject.Tile.environmentGameObject.EnvironmentGame.GetEnvironmentModifier(UnitGameObject.type);
        }
    }
}