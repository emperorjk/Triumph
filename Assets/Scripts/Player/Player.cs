using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
    public string name { get; private set; }
    public int gold { get; set; }
    public IList<BuildingsBase> ownedBuildings;
    public IList<UnitBase> ownedUnits;
    
    public Player(string name)
    {
        this.name = name;
    }

    public int GetCurrentIncome()
    {
        int temp = 0;
        foreach (BuildingsBase b in ownedBuildings) { temp += b.income; }
        return temp;
    }
}
