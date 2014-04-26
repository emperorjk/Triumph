using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.World
{
    /// <summary>
    /// This script is put on a prefab and creates the appropiate EnvironmentBase object. It also passes itself to the EnvironmentBase so they can both reach each other via a variable.
    /// </summary>
    public class EnvironmentGameObject : MonoBehaviour
    {
        public EnvironmentTypes type;
        public Environment EnvironmentGame { get; private set; }
        public Tile Tile { get; set; }

        private void Awake()
        {
            this.EnvironmentGame = GameJsonCreator.CreateEnvironment(this, type);
            Tile = GetComponent<Tile>();
            Tile.environmentGameObject = this;
        }
    }
}