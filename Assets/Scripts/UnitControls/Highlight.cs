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

    /// <summary>
    /// Gets called whenever an OnUnitClick event is fired.
    /// </summary>
    /// <param name="evt"></param>
    public void ShowHighlight(OnUnitClick evt)
    {
        if(evt.unit != null)
        {
            if (!IsHighlightOn && !_manager.Movement.needsMoving && !_manager.AnimInfo.IsAnimateFight)
            {
                UnitSelected = evt.unit;
                IsHighlightOn = true;
                if (!UnitSelected.UnitGame.hasMoved)
                {
                    UnitSelected.UnitGame.PlaySound(UnitSoundType.Select);

                    Dictionary<int, Dictionary<int, Tile>> movementListt = TileHelper.GetAllTilesWithinRange(UnitSelected.Tile.Coordinate, UnitSelected.UnitGame.MoveRange);
                    foreach (KeyValuePair<int, Dictionary<int, Tile>> item in movementListt)
                    {
                        foreach (KeyValuePair<int, Tile> tile in item.Value)
                        {
                            if (!tile.Value.HasUnit() && tile.Value.environmentGameObject.environmentGame.IsWalkable)
                            {
                                List<Node> path = _manager.Movement.CalculateShortestPath(UnitSelected.Tile, tile.Value, false);
                               
                                if (path != null && path.Count <= UnitSelected.UnitGame.MoveRange)
                                {
                                    tile.Value.highlight.ChangeHighlight(HighlightTypes.highlight_move);
                                    HighlightObjects.Add(tile.Value.highlight);
                                }
                            }  
                        }
                    }
                    _manager.Attack.ShowAttackHighlights(UnitSelected, UnitSelected.UnitGame.GetAttackMoveRange);
                }
                else if (UnitSelected.UnitGame.CanAttackAfterMove && !UnitSelected.UnitGame.hasAttacked)
                {
                    _manager.Attack.ShowAttackHighlights(UnitSelected, UnitSelected.UnitGame.AttackRange);
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
                    UnitSelected.UnitGame.hasMoved = true;
                    _manager.Movement.nodeList = _manager.Movement.CalculateShortestPath(UnitSelected.Tile, highlight.tile, false);
                    _manager.Movement.StartTimeMoving = Time.time;
                    _manager.Movement.needsMoving = true;
                    UnitSelected.UnitGame.PlaySound(UnitSoundType.Move);
                    ClearHighlights();
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
            unit.UpdateUnitColor();
        }
        ClearHighlights();
    }

    /// <summary>
    /// Clears only the highlights.
    /// </summary>
    public void ClearHighlights()
    {
        foreach (HighlightObject item in HighlightObjects)
        {
            item.ChangeHighlight(HighlightTypes.highlight_none);
        }
        HighlightObjects.Clear();
        IsHighlightOn = false;
    }
}