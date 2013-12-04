using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class MenuStateBase
{
    public abstract void OnUpdate();
    public abstract void OnActive();
    public abstract void OnInActive();
}
