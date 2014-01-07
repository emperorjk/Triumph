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
}

