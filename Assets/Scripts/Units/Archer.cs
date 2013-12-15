using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Archer : UnitBase
{
    public Archer(UnitGameObject game)
        : base(game, 10, 1.5f, 2, 2, 2)
    {

    }

    public override Sprite sprite
    {
        get
        {
            // Can be improved if the .png files are named archers1. This is the normal archer unit for player one.
            // We can append the playerindex e.g. 1 or 2 to the string archer and thus this eliminates the need for strings to check for players.
            Sprite s = null;
            if (unitGameObject.index == PlayerIndex.Neutral)
            {

            }
            else if (unitGameObject.index == PlayerIndex.One)
            {
                s = Resources.Load<Sprite>("Textures/Units/archers_blue");
            }
            else if (unitGameObject.index == PlayerIndex.Two)
            {
                s = Resources.Load<Sprite>("Textures/Units/archers_red");
            }

            return s;
        }
        protected set
        {
            throw new NotImplementedException();
        }
    }
}

