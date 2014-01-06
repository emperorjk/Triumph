using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class IUnitGameObject
{
    public string DirToUnitFolder { get { return "Prefabs/Units/"; } }
    public abstract GameObject CreateUnit(PlayerIndex index);
    public abstract GameObject CreateHeroUnit(PlayerIndex index);
}
