using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Highlight
{
    private GameManager _manager;
    private TextMesh _notificationText;
    private bool _notificationTextActivated = false;
    private float startTime = 1f;

    public Movement _movement;
    public Attack _attack;
    public List<HighlightObject> highlightObjects { get; private set; }
    public UnitGameObject _unitSelected { get; set; }
    public bool isHighlightOn { get; set; }

    public Highlight()
    {
        _manager = GameManager.Instance;
        _movement = new Movement();
        _attack = new Attack();
        isHighlightOn = false;
        highlightObjects = new List<HighlightObject>();
        _notificationText = GameObject.Find("NotificationText").GetComponent<TextMesh>();
        EventHandler.register<OnUnitClick>(ShowHighlight);
        EventHandler.register<OnHighlightClick>(ClickedOnHightLight);
    }

    public void OnUpdate()
    {
        if (_movement.needsMoving)
        {
            if (_movement.nodeList != null)
            {
                _movement.Moving(_unitSelected, _attack);
            }
        }

        // Reset notification text after some time.
        if (_notificationTextActivated)
        {
            startTime -= Time.deltaTime;

            if (startTime <= 0)
            {
                _notificationTextActivated = false;
                _notificationText.text = "";
            }
        }
    }

    /// <summary>
    /// Gets called whenever an OnUnitClick event is fired.
    /// </summary>
    /// <param name="evt"></param>
    public void ShowHighlight(OnUnitClick evt)
    {
        if(evt.unit != null)
        {
            if (!isHighlightOn && !_movement.needsMoving)
            {
                _unitSelected = evt.unit;
                _unitSelected.unitGame.PlaySound(UnitSoundType.Select);
                isHighlightOn = true;
                if (!_unitSelected.unitGame.hasMoved)
                {
                    Dictionary<int, Dictionary<int, Tile>> movementListt = _manager.GetAllTilesWithinRange(_unitSelected.tile.Coordinate, _unitSelected.unitGame.moveRange);
                    foreach (KeyValuePair<int, Dictionary<int, Tile>> item in movementListt)
                    {
                        foreach (KeyValuePair<int, Tile> tile in item.Value)
                        {
                            if (!tile.Value.HasUnit() && tile.Value.environmentGameObject.environmentGame.IsWalkable)
                            {
                                List<Node> path = _movement.CalculateShortestPath(_unitSelected.tile, tile.Value, false);
                               
                                if (path != null && path.Count <= _unitSelected.unitGame.moveRange)
                                {
                                    tile.Value.highlight.ChangeHighlight(HighlightTypes.highlight_move);
                                    highlightObjects.Add(tile.Value.highlight);
                                }
                            }  
                        }
                    }
                    _attack.ShowAttackHighlights(_unitSelected, _unitSelected.unitGame.GetAttackMoveRange, _movement);
                }
                else if (_unitSelected.unitGame.CanAttackAfterMove && !_unitSelected.unitGame.hasAttacked)
                {
                    _attack.ShowAttackHighlights(_unitSelected, _unitSelected.unitGame.attackRange, _movement);
                }
            }
        }
    }

    /// <summary>
    /// Gets called whenever an OnHighlightClick event is fired.
    /// </summary>
    /// <param name="evt"></param>
    public void ClickedOnHightLight(OnHighlightClick evt)
    {
        if(evt.highlight != null)
        {
            if (isHighlightOn)
            {
                HighlightObject highlight = evt.highlight;
                if (highlight.highlightTypeActive == HighlightTypes.highlight_move)
                {
                    _unitSelected.unitGame.hasMoved = true;
                    _movement.nodeList = _movement.CalculateShortestPath(_unitSelected.tile, highlight.tile, false);
                    _movement.StartTimeMoving = Time.time;
                    _movement.needsMoving = true;
                    _unitSelected.unitGame.PlaySound(UnitSoundType.Move);
                    ClearNewHighlights();
                }
                else if (highlight.highlightTypeActive == HighlightTypes.highlight_attack)
                {
                    startTime = 1f;
                    _notificationText.text = "Move to this unit to attack!";
                    _notificationTextActivated = true;
                }
            }
        }
    }
    /// <summary>
    /// Clears all of the movement and highlights.
    /// </summary>
    public void ClearMovementAndHighLights()
    {
        foreach (UnitBase unit in GameManager.Instance.CurrentPlayer.ownedUnits)
        {
            unit.hasMoved = false;
            unit.hasAttacked = false;
        }
        ClearNewHighlights();
    }

    /// <summary>
    /// Clears only the highlights.
    /// </summary>
    public void ClearNewHighlights()
    {
        foreach (HighlightObject item in highlightObjects)
        {
            item.ChangeHighlight(HighlightTypes.highlight_none);
        }
        highlightObjects.Clear();
        isHighlightOn = false;
    }
}