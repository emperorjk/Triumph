using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Archer : UnitBase
{
    public Archer(UnitGameObject game)
        : base(game, 10, 1.5f, 3, 3, 2, 100)
    {

    }

    public override bool CanAttackAfterMove
    {
        get { return false; }
    }

    public override int GetAttackRange
    {
        get { return attackRange; }
    }

    public override int FowLineOfSightRange
    {
        get { return 1; }
    }


    public override void PlaySound(string audio)
    {
        System.Random ran = new System.Random();

        switch (audio)
        { 
            case "move":
                if (ran.Next(2) == 0)
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.archerMove1);
                }
                else
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.archerMove2);
                }
                
                break;
            case "attack":
                if (ran.Next(2) == 0)
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.archerAttack1);
                }
                else
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.archerAttack2);
                }
                
                break;
            case "select":
                if (ran.Next(2) == 0)
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.archerSelect1);
                }
                else
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.archerSelect2);
                }
                break;
        }
    }
}