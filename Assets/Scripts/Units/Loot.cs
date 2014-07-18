using Assets.Scripts.Players;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Loot : MonoBehaviour
    {
        private int AmountTurnsDestroy { get; set; }
        public int CurrentTurnAmount { get; set; }
        public float AmountLoot { get; private set; }
        // We need a reference to the Tile the loot is on in order to clear the reference on the Tile object to the loot.
        public Tile Tile { get; set; }

        private void Awake()
        {
            AmountTurnsDestroy = 4;
        }

        /// <summary>
        /// After a number of turns destroy the Loot.
        /// </summary>
        public void IncreaseTurn()
        {
            CurrentTurnAmount++;

            if (CurrentTurnAmount >= AmountTurnsDestroy)
            {
                Destroy(gameObject);
            }
        }

        public void SetLoot(float loot)
        {
            AmountLoot += loot;
        }

        public void PickUpLoot(Player player)
        {
            player.IncreaseGoldBy(AmountLoot);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            Tile.Loot = null;
        }
    }
}