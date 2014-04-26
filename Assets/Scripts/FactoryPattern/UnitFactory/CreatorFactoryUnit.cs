﻿using System;
using Assets.Scripts.Players;
using Assets.Scripts.Tiles;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.FactoryPattern.UnitFactory
{
    public class CreatorFactoryUnit
    {
        private static UnitGameObject ConfigUnitAndTile(Tile tile, GameObject obj)
        {
            UnitGameObject unit = ((GameObject) GameObject.Instantiate(obj)).GetComponent<UnitGameObject>();
            tile.unitGameObject = unit;
            unit.transform.position = tile.transform.position;
            unit.transform.parent = tile.transform;
            unit.Tile = tile;
            return unit;
        }

        public static UnitGameObject CreateUnit(Tile tile, PlayerIndex index, UnitTypes type)
        {
            if (tile.HasUnit())
            {
                throw new ArgumentException("The given Tile already has a unit on it. Cannot spawn a unit.", "tile");
            }
            GameObject obj = null;
            if (type == UnitTypes.Archer)
            {
                ArcherFactory fac = new ArcherFactory();
                obj = fac.CreateUnit(index);
            }
            else if (type == UnitTypes.Knight)
            {
                KnightFactory fac = new KnightFactory();
                obj = fac.CreateUnit(index);
            }
            else if (type == UnitTypes.Swordsman)
            {
                SwordsmanFactory fac = new SwordsmanFactory();
                obj = fac.CreateUnit(index);
            }
            return ConfigUnitAndTile(tile, obj);
        }

        public static UnitGameObject CreateHeroUnit(Tile tile, PlayerIndex index, UnitTypes type)
        {
            if (tile.HasUnit())
            {
                throw new ArgumentException("The given Tile already has a unit on it. Cannot spawn a unit.", "tile");
            }
            GameObject obj = null;
            if (type == UnitTypes.Archer)
            {
                ArcherFactory fac = new ArcherFactory();
                obj = fac.CreateHeroUnit(index);
            }
            else if (type == UnitTypes.Knight)
            {
                KnightFactory fac = new KnightFactory();
                obj = fac.CreateHeroUnit(index);
            }
            else if (type == UnitTypes.Swordsman)
            {
                SwordsmanFactory fac = new SwordsmanFactory();
                obj = fac.CreateHeroUnit(index);
            }
            return ConfigUnitAndTile(tile, obj);
        }
    }
}