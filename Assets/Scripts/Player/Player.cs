using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player {
    public string name { get; private set; }
    public int gold { get; private set; }

    // public? We need to get the owned units to check collider hit
    public IList<BuildingsBase> ownedBuildings;
    public IList<UnitBase> ownedUnits;
    
    public Player(string name)
    {
        this.name = name;
        ownedBuildings = new List<BuildingsBase>();
        ownedUnits = new List<UnitBase>();
    }

    public void AddBuilding(BuildingsBase building)
    {
        if(!ownedBuildings.Contains(building)) { ownedBuildings.Add(building); }
    }

    public void AddUnit(UnitBase unit)
    {
        if (!ownedUnits.Contains(unit)) { ownedUnits.Add(unit); }
    }

    public void IncreaseGoldBy(int increaseBy) { gold += increaseBy; }

    public void DecreaseGoldBy(int decreaseBy) { gold -= decreaseBy; }
    
    /// <summary>
    /// Returns true is you have enough money to buy a unit.
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    public bool CanBuy(int money)
    {
        return (gold - money >= 0);
    }

    public int GetCurrentIncome()
    {
        // This should work. Haven't tested it yet.
        return ownedBuildings.Sum(x => x.income);
        //int temp = 0;
        //foreach (BuildingsBase b in ownedBuildings) { temp += b.income; }
        //return temp;
    }
}
