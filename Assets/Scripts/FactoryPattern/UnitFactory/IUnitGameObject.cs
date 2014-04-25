using Assets.Scripts.Main;
using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.FactoryPattern.UnitFactory
{
    public abstract class IUnitGameObject
    {
        public string DirToUnitFolder
        {
            get { return FileLocations.prefabUnits; }
        }

        public abstract GameObject CreateUnit(PlayerIndex index);
        public abstract GameObject CreateHeroUnit(PlayerIndex index);
    }
}