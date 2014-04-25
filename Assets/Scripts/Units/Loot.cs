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
        // We need a reference to the tile the loot is on in order to clear the reference on the tile object to the loot.
        public Tile tile { get; set; }

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
                Destroy(this.gameObject);
            }
        }

        public void SetLoot(float loot)
        {
            AmountLoot += loot;
        }

        public void PickUpLoot(Player player)
        {
            player.IncreaseGoldBy(AmountLoot);
            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            tile.Loot = null;
        }
    }
}