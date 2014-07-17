using System;
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
            IUnitGameObject go = null;

            if (type == UnitTypes.Archer) { go = new ArcherFactory(); }
            else if (type == UnitTypes.Knight) { go = new KnightFactory(); }
            else if (type == UnitTypes.Swordsman) { go = new SwordsmanFactory(); }
            obj = go.CreateUnit(index);
            return ConfigUnitAndTile(tile, obj);
        }

        public static UnitGameObject CreateHeroUnit(Tile tile, PlayerIndex index, UnitTypes type)
        {
            if (tile.HasUnit())
            {
                throw new ArgumentException("The given Tile already has a unit on it. Cannot spawn a herounit.", "tile");
            }
            GameObject obj = null;
            IUnitGameObject go = null;
            if (type == UnitTypes.Archer) { go = new ArcherFactory(); }
            else if (type == UnitTypes.Knight) { go = new KnightFactory(); }
            else if (type == UnitTypes.Swordsman) { go = new SwordsmanFactory(); }
            obj = go.CreateHeroUnit(index);
            return ConfigUnitAndTile(tile, obj);
        }
    }
}