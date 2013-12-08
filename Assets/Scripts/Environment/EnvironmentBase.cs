using UnityEngine;
using System.Collections;

public abstract class EnvironmentBase
{
    protected EnvironmentBase(EnvironmentGameObject game)
    {
        this.environmentGameObject = game;
    }

    public abstract EnvironmentTypes type { get; }
    public EnvironmentGameObject environmentGameObject { get; private set; }
}
