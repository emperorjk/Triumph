using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SimpleJSON;

public class GameJsonCreator
{
    public static Unit CreateUnit(UnitGameObject ug, bool isHero, UnitTypes type)
    {
        string jsonString = Resources.Load<TextAsset>("JSON/Units/" + type.ToString()).text;
        JSONNode jsonUnit = JSON.Parse(jsonString);

        int attackRange = jsonUnit["attackRange"].AsInt;
        int moveRange = jsonUnit["moveRange"].AsInt;
        bool canAttackAfterMove = jsonUnit["canAttackAfterMove"].AsBool;
        float maxHealth = jsonUnit["maxHealth"].AsFloat;
        float damage = jsonUnit["damage"].AsFloat;
        int cost = jsonUnit["cost"].AsInt;
        int fowLos = jsonUnit["fowLos"].AsInt;
        float baseLoot = jsonUnit["baseLoot"].AsFloat;
        JSONArray a = jsonUnit["unitModifiers"].AsArray;

        Dictionary<UnitTypes, float> modifiers = new Dictionary<UnitTypes, float>();

        foreach (UnitTypes suit in (UnitTypes[])Enum.GetValues(typeof(UnitTypes)))
        {
            foreach (JSONNode item in a)
            {
                if (item[suit.ToString()] != null && item[suit.ToString()] != "" && item[suit.ToString()] != suit.ToString())
                {
                    modifiers.Add(suit, item[suit.ToString()].AsFloat);
                }
            }
        }

        return new Unit(ug, isHero, attackRange, moveRange, canAttackAfterMove, maxHealth, damage, cost, fowLos, baseLoot, modifiers);
    }

    public static Building CreateBuilding(BuildingGameObject bg, BuildingTypes type)
    {
        string jsonString = Resources.Load<TextAsset>("JSON/Buildings/" + type.ToString()).text;
        JSONNode jsonBuilding = JSON.Parse(jsonString);

        int income = jsonBuilding["income"].AsInt;
        float capturePoints = jsonBuilding["capturePoints"].AsFloat;
        bool canProduce = jsonBuilding["canProduce"].AsBool;
        float damageToCapturingUnit = jsonBuilding["damageToCapturingUnit"].AsFloat;
        int fowLos = jsonBuilding["fowLos"].AsInt;
        int attackRange = jsonBuilding["attackRange"].AsInt;
        float damage = jsonBuilding["damage"].AsFloat;
        JSONArray a = jsonBuilding["unitModifiers"].AsArray;

        Dictionary<UnitTypes, float> modifiers = new Dictionary<UnitTypes, float>();

        foreach (UnitTypes suit in (UnitTypes[])Enum.GetValues(typeof(UnitTypes)))
        {
            foreach (JSONNode item in a)
            {
                if (item[suit.ToString()] != null && item[suit.ToString()] != "")
                {
                    modifiers.Add(suit, item[suit.ToString()].AsFloat);
                }
            }
        }

        return new Building(bg, income, capturePoints, canProduce, damageToCapturingUnit, fowLos, attackRange, damage, modifiers);
    }

    public static Environment CreateEnvironment(EnvironmentGameObject eg, EnvironmentTypes type)
    {
        string jsonString = Resources.Load<TextAsset>("JSON/Environments/" + type.ToString()).text;
        JSONNode jsonEnvironment = JSON.Parse(jsonString);

        bool isWalkable = jsonEnvironment["isWalkable"].AsBool;

        JSONArray a = jsonEnvironment["unitModifiers"].AsArray;
        
        Dictionary<UnitTypes, float> modifiers = new Dictionary<UnitTypes,float>();

        foreach (UnitTypes suit in (UnitTypes[])Enum.GetValues(typeof(UnitTypes)))
        {
            foreach (JSONNode item in a)
            {
                if (item[suit.ToString()] != null && item[suit.ToString()] != "")
                {
                    modifiers.Add(suit, item[suit.ToString()].AsFloat);
                }
            }
        }
        
        return new Environment(eg, isWalkable, modifiers);
    }
}