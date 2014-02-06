using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

public struct OnAnimFight
{
    public UnitGameObject attacker;
    public UnitGameObject defender;
    public bool needsAnimating;

    public OnAnimFight(UnitGameObject _attacker, UnitGameObject _defender, bool _needsAnimating)
    {
        attacker = _attacker;
        defender = _defender;
        needsAnimating = _needsAnimating;
    }
}

public struct OnSwipeAction
{
    public bool SwipeLeft;
    public bool SwipeRight;
    public bool SwipeUp;
    public bool SwipeDown;

    public OnSwipeAction(bool _SwipeLeft, bool _SwipeRight, bool _SwipeUp, bool _SwipDown)
    {
        SwipeLeft = _SwipeLeft;
        SwipeRight = _SwipeRight;
        SwipeUp = _SwipeUp;
        SwipeDown = _SwipDown;
    }
}