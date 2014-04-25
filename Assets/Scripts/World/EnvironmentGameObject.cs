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
        public Environment environmentGame { get; private set; }
        public Tile tile { get; set; }

        private void Awake()
        {
            this.environmentGame = GameJsonCreator.CreateEnvironment(this, type);
            tile = GetComponent<Tile>();
            tile.environmentGameObject = this;
        }
    }
}