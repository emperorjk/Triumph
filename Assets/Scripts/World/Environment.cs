using System.Collections.Generic;
using Assets.Scripts.Units;

namespace Assets.Scripts.World
{
    public class Environment
    {
        public Environment(EnvironmentGameObject game, bool isWalkable, Dictionary<UnitTypes, float> modifiers)
        {
            this.environmentGameObject = game;
            this.IsWalkable = isWalkable;
            this.modifiers = modifiers;
        }

        public EnvironmentGameObject environmentGameObject { get; private set; }
        public bool IsWalkable { get; set; }
        private Dictionary<UnitTypes, float> modifiers { get; set; }

        /// <summary>
        /// Returns the environment modifier for the given UnitTypes.
        /// This method should not be called directly. The Unit class has the method GetGroundModifier() which does some checks and if need be calls this method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public float GetEnvironmentModifier(UnitTypes type)
        {
            return modifiers[type];
        }
    }
}