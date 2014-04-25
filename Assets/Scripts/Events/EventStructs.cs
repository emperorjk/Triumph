using Assets.Scripts.Buildings;
using Assets.Scripts.UnitActions;
using Assets.Scripts.Units;

namespace Assets.Scripts.Events
{
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
        public int fingerCount;
        public bool SwipeLeft;
        public bool SwipeRight;
        public bool SwipeUp;
        public bool SwipeDown;

        public OnSwipeAction(int _fingerCount, bool _SwipeLeft, bool _SwipeRight, bool _SwipeUp, bool _SwipDown)
        {
            fingerCount = _fingerCount;
            SwipeLeft = _SwipeLeft;
            SwipeRight = _SwipeRight;
            SwipeUp = _SwipeUp;
            SwipeDown = _SwipDown;
        }
    }
}