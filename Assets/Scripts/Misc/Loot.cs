using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour 
{
    private int AmountTurnsDestroy { get; set; }
    public int CurrentTurnAmount { get; set; }
    public int AmountLoot { get; private set; }

    void Awake()
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

    public void SetLoot(int loot)
    {
        AmountLoot += loot;
    }

    public void PickUpLoot(Player player)
    {
        player.IncreaseGoldBy(AmountLoot);
        Destroy(this.gameObject);
    }
}