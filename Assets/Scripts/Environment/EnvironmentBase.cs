using UnityEngine;
using System.Collections;

public abstract class EnvironmentBase
{
    protected EnvironmentBase(EnvironmentGameObject game)
    {
        this.environmentGameObject = game;
    }
    public abstract bool IsWalkable { get; }
    public abstract EnvironmentTypes type { get; }
    public EnvironmentGameObject environmentGameObject { get; private set; }
}
