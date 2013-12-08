using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This script is put on a prefab and creates the appropiate UnitBase object. It also passes itself to the UnitBase so they can both reach each other via a variable.
/// And adds the UnitBase object the the correct player list of owned units.
/// </summary>
public class UnitGameObject : MonoBehaviour
{
    public PlayerIndex index;
    public UnitTypes type;
    public UnitBase unitGame { get; private set; }

	void Start () {
        // for now ugly code
        if (type.Equals(UnitTypes.Archer)) { unitGame = new Archer(this); }
        else if (type.Equals(UnitTypes.Knight)) { unitGame = new Knight(this); }
        else if (type.Equals(UnitTypes.Swordsman)) { unitGame = new Swordsman(this); }
        GameManager.Instance.GetPlayer(index).AddUnit(unitGame);
	}
}
