using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Highlight
{
    public List<HighlightObject> HighlightObjects { get; private set; }
    public UnitGameObject UnitSelected { get; set; }
    public bool IsHighlightOn { get; set; }
    private GameManager _manager;

    public Highlight()
    {
        _manager = GameManager.Instance;
        IsHighlightOn = false;
        HighlightObjects = new List<HighlightObject>();
        EventHandler.register<OnUnitClick>(ShowHighlight);
        EventHandler.register<OnHighlightClick>(ClickedOnHightLight);
    }

    public void OnUpdate()
    {
        if (_manager.Movement.needsMoving)
        {
            if (_manager.Movement.nodeList != null)
            {
                _manager.Movement.Moving(UnitSelected, _manager.Attack);
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
            if (!IsHighlightOn && !_manager.Movement.needsMoving)
            {
                UnitSelected = evt.unit;
                IsHighlightOn = true;
                if (!UnitSelected.unitGame.hasMoved)
                {
                    UnitSelected.unitGame.PlaySound(UnitSoundType.Select);

                    Dictionary<int, Dictionary<int, Tile>> movementListt = TileHelper.GetAllTilesWithinRange(UnitSelected.tile.Coordinate, UnitSelected.unitGame.moveRange);
                    foreach (KeyValuePair<int, Dictionary<int, Tile>> item in movementListt)
                    {
                        foreach (KeyValuePair<int, Tile> tile in item.Value)
                        {
                            if (!tile.Value.HasUnit() && tile.Value.environmentGameObject.environmentGame.IsWalkable)
                            {
                                List<Node> path = _manager.Movement.CalculateShortestPath(UnitSelected.tile, tile.Value, false);
                               
                                if (path != null && path.Count <= UnitSelected.unitGame.moveRange)
                                {
                                    tile.Value.highlight.ChangeHighlight(HighlightTypes.highlight_move);
                                    HighlightObjects.Add(tile.Value.highlight);
                                }
                            }  
                        }
                    }
                    _manager.Attack.ShowAttackHighlights(UnitSelected, UnitSelected.unitGame.GetAttackMoveRange);
                }
                else if (UnitSelected.unitGame.CanAttackAfterMove && !UnitSelected.unitGame.hasAttacked)
                {
                    _manager.Attack.ShowAttackHighlights(UnitSelected, UnitSelected.unitGame.attackRange);
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
            if (IsHighlightOn)
            {
                HighlightObject highlight = evt.highlight;
                if (highlight.highlightTypeActive == HighlightTypes.highlight_move)
                {
                    UnitSelected.unitGame.hasMoved = true;
                    _manager.Movement.nodeList = _manager.Movement.CalculateShortestPath(UnitSelected.tile, highlight.tile, false);
                    _manager.Movement.StartTimeMoving = Time.time;
                    _manager.Movement.needsMoving = true;
                    UnitSelected.unitGame.PlaySound(UnitSoundType.Move);
                    ClearNewHighlights();
                }
                else if (highlight.highlightTypeActive == HighlightTypes.highlight_attack)
                {
                    Notificator.Notify("Move to this unit to attack!", 1f);
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
        foreach (HighlightObject item in HighlightObjects)
        {
            item.ChangeHighlight(HighlightTypes.highlight_none);
        }
        HighlightObjects.Clear();
        IsHighlightOn = false;
    }
}