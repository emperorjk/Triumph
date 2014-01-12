using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Swordsman : UnitBase
{
    public Swordsman(UnitGameObject game) 
        : base(game, 10, 1.0f, 1, 1, 2, 150)
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
                    GameManager.Instance.Sounds.PlaySound(Sounds.swordsmanMove1);
                }
                else
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.swordsmanMove2);
                }

                break;
            case "attack":
                if (ran.Next(2) == 0)
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.swordsmanAttack1);
                }
                else
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.swordsmanAttack2);
                }

                break;
            case "select":
                if (ran.Next(2) == 0)
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.swordsmanSelect1);
                }
                else
                {
                    GameManager.Instance.Sounds.PlaySound(Sounds.swordsmanSelect2);
                }
                break;
        }
    }
}

