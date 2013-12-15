using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Knight : UnitBase
{
    public Knight(UnitGameObject game) 
        : base(game, 10, 1.5f, 1, 1, 3)
    {

    }

    public override Sprite sprite
    {
        get
        {
            throw new NotImplementedException();
        }
        protected set
        {
            throw new NotImplementedException();
        }
    }
}

