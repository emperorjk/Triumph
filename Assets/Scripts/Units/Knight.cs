using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Knight : UnitBase
{
    public Knight(UnitGameObject game) 
        : base(game, 10, 1.5f, 1, 1, 3, 250)
    {
        
    }

    public override bool CanAttackAfterMove
    {
        get { return true; }
    }

    public override int GetAttackRange
    {
        get { return attackRange + moveRange; }
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
                    GameManager.Instance.sounds.PlaySound(Sounds.knightMove1);
                }
                else
                {
                    GameManager.Instance.sounds.PlaySound(Sounds.knightMove2);
                }

                break;
            case "attack":
                if (ran.Next(2) == 0)
                {
                    GameManager.Instance.sounds.PlaySound(Sounds.knightAttack1);
                }
                else
                {
                    GameManager.Instance.sounds.PlaySound(Sounds.knightAttack2);
                }

                break;
            case "select":
                if (ran.Next(2) == 0)
                {
                    GameManager.Instance.sounds.PlaySound(Sounds.knightSelect1);
                }
                else
                {
                    GameManager.Instance.sounds.PlaySound(Sounds.knightSelect2);
                }

                break;
        }
    }
}