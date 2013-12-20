using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This script is put on a prefab and creates the appropiate EnvironmentBase object. It also passes itself to the EnvironmentBase so they can both reach each other via a variable.
/// </summary>
public class EnvironmentGameObject : MonoBehaviour
{
    public EnvironmentTypes type;
    public EnvironmentBase environmentGame { get; private set; }
    public Tile tile { get; set; }

	void Start () {
        // for now ugly code
        if (type.Equals(EnvironmentTypes.Big_Rocks)) { environmentGame = new Big_Rocks(this); }
        else if (type.Equals(EnvironmentTypes.Bridge)) { environmentGame = new Bridge(this); }
        else if (type.Equals(EnvironmentTypes.Forrest)) { environmentGame = new Forrest(this); }
        else if (type.Equals(EnvironmentTypes.Grass)) { environmentGame = new Grass(this); }
        else if (type.Equals(EnvironmentTypes.Road)) { environmentGame = new Road(this); }
        else if (type.Equals(EnvironmentTypes.Small_Rocks)) { environmentGame = new Small_Rocks(this); }
        else if (type.Equals(EnvironmentTypes.Water)) { environmentGame = new Water(this); }
	}
}
