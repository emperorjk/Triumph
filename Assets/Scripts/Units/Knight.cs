using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Knight : UnitBase
{
    public Knight(UnitGameObject game, bool isHero)
        : base(game, 10, 1.5f, 1, 3, 250, isHero)
    {
        
    }

    public override bool CanAttackAfterMove
    {
        get { return true; }
    }

    public override int GetAttackMoveRange
    {
        get { return GameManager.Instance.FowManager.isFowActive ? AttackRange : AttackRange + MoveRange; }
    }

    public override int FowLineOfSightRange
    {
        get { return 1; }
    }

    public override void PlaySound(UnitSoundType soundType)
    {
        GameManager.Instance.UnitSounds.PlaySound(this.UnitGameObject.type, soundType);
    }
}