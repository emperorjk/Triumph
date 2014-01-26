using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// This "fake" class holds all of the structs which are used for events.

public struct OnUnitClick
{
    public UnitGameObject unit;
    public OnUnitClick(UnitGameObject _unit)
    {
        unit = _unit;
    }
}

public struct OnBuildingClick
{
    public BuildingGameObject building;
    public OnBuildingClick(BuildingGameObject _building)
    {
        building = _building;
    }
}

public struct OnHighlightClick
{
    public HighlightObject highlight;
    public OnHighlightClick(HighlightObject _highlight)
    {
        highlight = _highlight;
    }
}