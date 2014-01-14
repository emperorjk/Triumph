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


    public override void PlaySound(UnitSoundType soundType)
    {
        GameManager.Instance.sounds.PlaySound(this.unitGameObject.type, soundType);
    }
}