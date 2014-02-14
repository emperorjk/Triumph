using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player {

    public PlayerIndex index { get; private set; }
    public string name { get; private set; }
    public float gold { get; private set; }
    public IList<Building> ownedBuildings { get; private set; }
    public IList<Unit> ownedUnits { get; private set; }
    public Color PlayerColor { get; private set; }
    public Player(string name, PlayerIndex index, Color playerColor)
    {
        this.name = name;
        this.index = index;
        this.PlayerColor = playerColor;
        ownedBuildings = new List<Building>();
        ownedUnits = new List<Unit>();
    }

    public void AddBuilding(Building building)
    {
        if(!ownedBuildings.Contains(building)) { ownedBuildings.Add(building); }
    }

    public void RemoveBuilding(Building building)
    {
        if (ownedBuildings.Contains(building)) { ownedBuildings.Remove(building); }
    }

    public void AddUnit(Unit unit)
    {
        if (!ownedUnits.Contains(unit)) { ownedUnits.Add(unit); }
    }

    public void RemoveUnit(Unit unit)
    {
        if (ownedUnits.Contains(unit)) { ownedUnits.Remove(unit); }
    }

    public void IncreaseGoldBy(float increaseBy) { gold += increaseBy; }

    public void DecreaseGoldBy(float decreaseBy) { gold -= decreaseBy; }
    
    /// <summary>
    /// Returns true is you have enough money to buy a unit.
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    public bool CanBuy(int cost)
    {
        return (gold - cost >= 0);
    }

    public int GetCurrentIncome()
    {
        return ownedBuildings.Sum(x => x.income);
    }
}
