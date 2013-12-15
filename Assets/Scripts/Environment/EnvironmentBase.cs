using UnityEngine;
using System.Collections;

public abstract class EnvironmentBase
{
    protected EnvironmentBase(EnvironmentGameObject game)
    {
        this.environmentGameObject = game;
    }
    /// <summary>
    /// Returns the sprite. Might change to texture or texture 2d if needed. For now this works.
    /// </summary>
    public abstract Sprite sprite { get; protected set; }

    public abstract EnvironmentTypes type { get; }
    public EnvironmentGameObject environmentGameObject { get; private set; }
}
