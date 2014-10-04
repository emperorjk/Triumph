using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Events;
using Assets.Scripts.Main;
using Assets.Scripts.Tiles;
using Assets.Scripts.Units;
using UnityEngine;
using EventHandler = Assets.Scripts.Events.EventHandler;
using Assets.Scripts.Levels;

namespace Assets.Scripts.UnitActions
{
    public class Highlight : MonoBehaviour
    {
        public List<HighlightObject> HighlightObjects { get; private set; }
        public UnitGameObject UnitSelected { get; set; }
        public bool IsHighlightOn { get; set; }

        private Movement movement;
        private Attack attack;
        private AnimationInfo animInfo;
        private LevelManager levelManager;

        private void Awake()
        {
            levelManager = GameObjectReferences.GetGlobalScriptsGameObject().GetComponent<LevelManager>();

            movement = GameObjectReferences.GetScriptsGameObject().GetComponent<Movement>();
            movement = GameObjectReferences.GetScriptsGameObject().GetComponent<Movement>();
            attack = GameObjectReferences.GetScriptsGameObject().GetComponent<Attack>();
            animInfo = GameObjectReferences.GetScriptsGameObject().GetComponent<AnimationInfo>();

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
                if (!IsHighlightOn && !movement.NeedsMoving && !animInfo.IsAnimateFight)
                {
                    UnitSelected = evt.unit;
                    IsHighlightOn = true;

                    if (!UnitSelected.UnitGame.HasMoved)
                    {
                        UnitSelected.UnitGame.PlaySound(UnitSoundType.Select);
                        UnitSelected.SelectionBox.enabled = true;

                        var movementListt = TileHelper.GetAllTilesWithinRange(UnitSelected.Tile.Coordinate,
                                UnitSelected.UnitGame.MoveRange);
                        foreach (var item in movementListt)
                        {
                            foreach (KeyValuePair<int, Tile> tile in item.Value)
                            {
                                if (!tile.Value.HasUnit() && tile.Value.environmentGameObject.EnvironmentGame.IsWalkable)
                                {
                                    List<Node> path = movement.CalculateShortestPath(UnitSelected.Tile,
                                        tile.Value, false);

                                    if (path != null && path.Count <= UnitSelected.UnitGame.MoveRange)
                                    {
                                        tile.Value.Highlight.ChangeHighlight(HighlightTypes.highlight_move);
                                        HighlightObjects.Add(tile.Value.Highlight);
                                    }
                                }
                            }
                        }
                        attack.ShowAttackHighlights(UnitSelected, UnitSelected.UnitGame.GetAttackMoveRange);
                    }
                    else if (UnitSelected.UnitGame.CanAttackAfterMove && !UnitSelected.UnitGame.HasAttacked)
                    {
                        attack.ShowAttackHighlights(UnitSelected, UnitSelected.UnitGame.AttackRange);
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
            if (evt.highlight != null && IsHighlightOn)
            {
                HighlightObject highlight = evt.highlight;

                if (highlight.highlightTypeActive == HighlightTypes.highlight_move)
                {
                    UnitSelected.UnitGame.HasMoved = true;
                    movement.nodeList = movement.CalculateShortestPath(UnitSelected.Tile,
                        highlight.Tile, false);
                    movement.StartTimeMoving = Time.time;
                    movement.NeedsMoving = true;
                    movement.FacingDirectionMovement(UnitSelected, movement.nodeList[0].Tile);
                    UnitSelected.UnitGame.PlaySound(UnitSoundType.Move);
                    ClearHighlights();
                }
            }
        }

        /// <summary>
        /// Clears all of the movement and highlights.
        /// </summary>
        public void ClearMovementAndHighLights()
        {
            foreach (Unit unit in levelManager.CurrentLevel.CurrentPlayer.OwnedUnits)
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