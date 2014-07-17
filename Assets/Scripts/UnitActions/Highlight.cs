using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Events;
using Assets.Scripts.Main;
using Assets.Scripts.Tiles;
using Assets.Scripts.Units;
using UnityEngine;
using EventHandler = Assets.Scripts.Events.EventHandler;

namespace Assets.Scripts.UnitActions
{
    public class Highlight : MonoBehaviour
    {
        public List<HighlightObject> HighlightObjects { get; private set; }
        public UnitGameObject UnitSelected { get; set; }
        public bool IsHighlightOn { get; set; }
        private GameManager _manager;

        private void Awake()
        {
            _manager = GameObject.Find("_Scripts").GetComponent<GameManager>();
            IsHighlightOn = false;
            HighlightObjects = new List<HighlightObject>();
            EventHandler.register<OnUnitClick>(ShowHighlight);
            EventHandler.register<OnHighlightClick>(ClickedOnHightLight);
        }

        private void OnDestroy()
        {
            EventHandler.unregister<OnUnitClick>(ShowHighlight);
            EventHandler.unregister<OnHighlightClick>(ClickedOnHightLight);
        }

        /// <summary>
        /// Gets called whenever an OnUnitClick event is fired.
        /// </summary>
        /// <param Name="evt"></param>
        public void ShowHighlight(OnUnitClick evt)
        {
            if (evt.unit != null)
            {

                if (!IsHighlightOn && !_manager.Movement.NeedsMoving && !_manager.AnimInfo.IsAnimateFight)
                {
                    UnitSelected = evt.unit;
                    IsHighlightOn = true;

                    if (!UnitSelected.UnitGame.HasMoved)
                    {
                        UnitSelected.UnitGame.PlaySound(UnitSoundType.Select);
                        UnitSelected.SelectionBox.enabled = true;

                        Dictionary<int, Dictionary<int, Tile>> movementListt =
                            TileHelper.GetAllTilesWithinRange(UnitSelected.Tile.Coordinate,
                                UnitSelected.UnitGame.MoveRange);
                        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in movementListt)
                        {
                            foreach (KeyValuePair<int, Tile> tile in item.Value)
                            {
                                if (!tile.Value.HasUnit() && tile.Value.environmentGameObject.EnvironmentGame.IsWalkable)
                                {
                                    List<Node> path = _manager.Movement.CalculateShortestPath(UnitSelected.Tile,
                                        tile.Value, false);

                                    if (path != null && path.Count <= UnitSelected.UnitGame.MoveRange)
                                    {
                                        tile.Value.Highlight.ChangeHighlight(HighlightTypes.highlight_move);
                                        HighlightObjects.Add(tile.Value.Highlight);
                                    }
                                }
                            }
                        }
                        _manager.Attack.ShowAttackHighlights(UnitSelected, UnitSelected.UnitGame.GetAttackMoveRange);
                    }
                    else if (UnitSelected.UnitGame.CanAttackAfterMove && !UnitSelected.UnitGame.HasAttacked)
                    {
                        _manager.Attack.ShowAttackHighlights(UnitSelected, UnitSelected.UnitGame.AttackRange);
                    }
                }
            }
        }

        /// <summary>
        /// Gets called whenever an OnHighlightClick event is fired.
        /// </summary>
        /// <param Name="evt"></param>
        public void ClickedOnHightLight(OnHighlightClick evt)
        {
            if (evt.highlight != null)
            {
                if (IsHighlightOn)
                {
                    HighlightObject highlight = evt.highlight;
                    if (highlight.highlightTypeActive == HighlightTypes.highlight_move)
                    {
                        UnitSelected.UnitGame.HasMoved = true;
                        _manager.Movement.nodeList = _manager.Movement.CalculateShortestPath(UnitSelected.Tile,
                            highlight.Tile, false);
                        _manager.Movement.StartTimeMoving = Time.time;
                        _manager.Movement.NeedsMoving = true;
                        _manager.Movement.FacingDirectionMovement(UnitSelected, _manager.Movement.nodeList[0].Tile);
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
            foreach (Unit unit in _manager.CurrentPlayer.OwnedUnits)
            {
                unit.HasMoved = false;
                unit.HasAttacked = false;
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
                UnitSelected.SelectionBox.enabled = false;
                item.ChangeHighlight(HighlightTypes.highlight_none);
            }
            HighlightObjects.Clear();
            IsHighlightOn = false;
        }
    }
}