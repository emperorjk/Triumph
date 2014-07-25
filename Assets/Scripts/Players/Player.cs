using Assets.Scripts.Buildings;
using Assets.Scripts.Units;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Players
{
    public class Player
    {
        public PlayerIndex Index { get; private set; }
        public string Name { get; private set; }
        public float Gold { get; private set; }
        public IList<Building> OwnedBuildings { get; private set; }
        public IList<Unit> OwnedUnits { get; private set; }
        public Color PlayerColor { get; private set; }

        public Player(string name, PlayerIndex index, Color playerColor)
        {
            Name = name;
            Index = index;
            PlayerColor = playerColor;
            OwnedBuildings = new List<Building>();
            OwnedUnits = new List<Unit>();
        }

        public void AddBuilding(Building building)
        {
            if (!OwnedBuildings.Contains(building))
            {
                OwnedBuildings.Add(building);
            }
        }

        public void RemoveBuilding(Building building)
        {
            if (OwnedBuildings.Contains(building))
            {
                OwnedBuildings.Remove(building);
            }
        }

        public void AddUnit(Unit unit)
        {
            if (!OwnedUnits.Contains(unit))
            {
                OwnedUnits.Add(unit);
            }
        }

        public void RemoveUnit(Unit unit)
        {
            if (OwnedUnits.Contains(unit))
            {
                OwnedUnits.Remove(unit);
            }
        }

        public void IncreaseGoldBy(float increaseBy)
        {
            Gold += increaseBy;
        }

        public void DecreaseGoldBy(float decreaseBy)
        {
            Gold -= decreaseBy;
        }

        public void SetGold(float gold)
        {
            Gold = gold;
        }

        /// <summary>
        /// Returns true is you have enough money to buy a unit.
        /// </summary>
        /// <param Name="money"></param>
        /// <returns></returns>
        public bool CanBuy(int cost)
        {
            return (Gold - cost >= 0);
        }

        public int GetCurrentIncome()
        {
            return OwnedBuildings.Sum(x => x.Income);
        }
    }
}