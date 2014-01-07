﻿using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This script is put on a prefab and creates the appropiate BuildingBase object. It also passes itself to the BuildingBase so they can both reach each other via a variable.
/// And adds the BuildingBase object the the correct player list of owned buildings.
/// </summary>
public class BuildingGameObject : MonoBehaviour
{
    public PlayerIndex index;
    public BuildingTypes type;
    public BuildingsBase buildingGame { get; private set; }
    public Tile tile { get; set; }
	void Awake () {
        // for now ugly code
        if (type.Equals(BuildingTypes.BarracksCavalry)) { buildingGame = new BarracksCavalry(this); }
        else if (type.Equals(BuildingTypes.BarracksMelee)) { buildingGame = new BarracksMelee(this); }
        else if (type.Equals(BuildingTypes.BarracksRange)) { buildingGame = new BarracksRange(this); }
        else if (type.Equals(BuildingTypes.Castle)) { buildingGame = new Castle(this); }
        else if (type.Equals(BuildingTypes.Headquarters)) { buildingGame = new Headquarters(this); }
        else if (type.Equals(BuildingTypes.TrainingZone)) { buildingGame = new TrainingZone(this); }
        if(this.transform.parent != null)
        {
            tile = this.transform.parent.GetComponent<Tile>();
        }
        GameManager.Instance.GetPlayer(index).AddBuilding(buildingGame);
	}
}
