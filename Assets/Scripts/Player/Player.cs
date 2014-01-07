using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player {

    public PlayerIndex index { get; private set; }
    public string name { get; private set; }
    public int gold { get; private set; }
    public IList<BuildingsBase> ownedBuildings { get; private set; }
    public IList<UnitBase> ownedUnits { get; private set; }
    
    public Player(string name, PlayerIndex index)
    {
        this.name = name;
        this.index = index;
        ownedBuildings = new List<BuildingsBase>();
        ownedUnits = new List<UnitBase>();
    }

    public void AddBuilding(BuildingsBase building)
    {
        if(!ownedBuildings.Contains(building)) { ownedBuildings.Add(building); }
    }

    public void RemoveBuilding(BuildingsBase building)
    {
        if (ownedBuildings.Contains(building)) { ownedBuildings.Remove(building); }
    }

    public void AddUnit(UnitBase unit)
    {
        if (!ownedUnits.Contains(unit)) { ownedUnits.Add(unit); }
    }

    public void RemoveUnit(UnitBase unit)
    {
        if (ownedUnits.Contains(unit)) 
        {
            ownedUnits.Remove(unit); 
        }
    }

    public void IncreaseGoldBy(int increaseBy) { gold += increaseBy; }

    public void DecreaseGoldBy(int decreaseBy) { gold -= decreaseBy; }
    
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
        // This should work. Haven't tested it yet.
        return ownedBuildings.Sum(x => x.income);
        //int temp = 0;
        //foreach (BuildingsBase b in ownedBuildings) { temp += b.income; }
        //return temp;
    }
}
