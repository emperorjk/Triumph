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
}

