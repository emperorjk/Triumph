﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Main;
using Assets.Scripts.Tiles;
using Assets.Scripts.Units;
using UnityEngine;
using Assets.Scripts.Levels;
using Assets.Scripts.Buildings;
using Assets.Scripts.DayNight;

namespace Assets.Scripts.UnitActions
{
    public class Movement : MonoBehaviour
    {
        public float StartTimeMoving { get; set; }
        public bool NeedsMoving { get; set; }
        public List<Node> nodeList;

        private readonly CompareNodes compare = new CompareNodes();
        private const float movingDuration = 0.65f;

        private Highlight highlight;
        private Attack attack;
        private DayStateController dayStateControl;
        private LevelManager levelManager;

        private void Start()
        {
            levelManager = GameObjectReferences.GetGlobalScriptsGameObject().GetComponent<LevelManager>();
            highlight = GameObjectReferences.GetScriptsGameObject().GetComponent<Highlight>();
            attack = GameObjectReferences.GetScriptsGameObject().GetComponent<Attack>();
            dayStateControl = GameObjectReferences.GetScriptsGameObject().GetComponent<DayStateController>();
        }

        private void Update()
        {
            if (NeedsMoving && nodeList != null)
            {
                Moving(highlight.UnitSelected);
            }
        }

        public void Moving(UnitGameObject unitMoving)
        {
            unitMoving.transform.position = Vector2.Lerp(unitMoving.Tile.Vector2, nodeList.Last().Tile.Vector2,
                GetTimePassed());

            if (GetTimePassed() >= 1f)
            {
                // Show fow for the unit.
                dayStateControl.ShowFowWithinLineOfSight(unitMoving.index);
                // Remove the references from the old Tile.
                unitMoving.Tile.unitGameObject = null;
                unitMoving.Tile = null;
                // Remove the last Tile from the list.
                Tile newPosition = nodeList.Last().Tile;
                nodeList.Remove(nodeList.Last());
                // Assign the references using the new Tile.
                newPosition.unitGameObject = unitMoving;
                unitMoving.Tile = newPosition;
                unitMoving.Tile.Vector2 = newPosition.Vector2;
                // Set the parent and position of the unit to the new Tile.
                unitMoving.transform.parent = newPosition.transform;
                unitMoving.transform.position = newPosition.transform.position;
                // Hide the fow for the unit. It will use the new Tile location.
                dayStateControl.HideFowWithinLineOfSight(unitMoving.index);
                StartTimeMoving = Time.time;
            }

            if (nodeList.Count <= 0)
            {
                highlight.ClearHighlights();
                Tile endDestinationTile = unitMoving.Tile;

                if (endDestinationTile.HasLoot())
                {
                    endDestinationTile.Loot.PickUpLoot(levelManager.CurrentLevel.CurrentPlayer);
                }

                if (endDestinationTile.HasBuilding())
                {
                    CaptureBuildings capBuilding = GameObject.Find("_Scripts").GetComponent<CaptureBuildings>();
                    capBuilding.AddBuildingToCaptureList(
                        endDestinationTile.buildingGameObject.BuildingGame);
                }
                if (unitMoving.UnitGame.CanAttackAfterMove &&
                    attack.ShowAttackHighlights(unitMoving, unitMoving.UnitGame.AttackRange) > 0)
                {
                    unitMoving.UnitGame.HasMoved = true;
                }
                else
                {
                    unitMoving.UnitGame.HasMoved = true;
                    unitMoving.UnitGame.HasAttacked = true;
                }
                NeedsMoving = false;
                unitMoving.UnitGame.UpdateUnitColor();
            }
        }

        public void FacingDirectionMovement(UnitGameObject moveUnit, Tile destination)
        {
            Vector3 direction = moveUnit.transform.position - destination.transform.position;

            var quaternion = new Quaternion(0, (direction.x > 0 ? 180 : 0), 0, 0);
            moveUnit.transform.rotation = quaternion;

            var attackerHealthQ = new Quaternion(0, 0, 0, (moveUnit.transform.position.y > 0 ? 0 : 180));
            moveUnit.UnitHealthText.transform.rotation = attackerHealthQ;
        }

        private float GetTimePassed()
        {
            return (Time.time - StartTimeMoving)/movingDuration;
        }

        /// <summary>
        /// Calculates the shortest path with the A* search algorithm.
        /// </summary>
        /// <param Name="start"></param>
        /// <param Name="goal"></param>
        /// <returns></returns>
        public List<Node> CalculateShortestPath(Tile start, Tile goal, bool attackCalculate)
        {
            var openList = new List<Node>();
            var closedList = new List<Node>();
            var current = new Node(start.Vector2, start, null, 0, 0);
            int maxValueCounter = 0;

            openList.Add(current);
            while (openList.Count > 0)
            {
                openList.Sort(compare);
                current = openList[0];

                if (current.Tile.Equals(goal))
                {
                    var path = new List<Node>();

                    while (current.Parent != null)
                    {
                        path.Add(current);
                        current = current.Parent;
                    }
                    openList.Clear();
                    closedList.Clear();

                    return path;
                }

                openList.Remove(current);
                closedList.Add(current);
                for (int i = 0; i < 9; i++)
                {
                    int x = (i%3) - 1;
                    int y = (i/3) - 1;

                    // This line prevents diagonal movement
                    if (!((x == 0 && y != 0) || (x != 0 && y == 0)))
                    {
                        continue;
                    }

                    Tile t =
                        TileHelper.GetTile(new TileCoordinates(x + current.Tile.Coordinate.ColumnId,
                            y + current.Tile.Coordinate.RowId));

                    if (t == null)
                    {
                        continue;
                    }
                    if (attackCalculate)
                    {
                        // if T is not equal to goal we want to check if isWalkable and HasUnit. Otherwise we cannot move to goal because that one has an unit.
                        if (!t.Equals(goal))
                        {
                            if (!t.environmentGameObject.EnvironmentGame.IsWalkable || t.HasUnit())
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (!t.environmentGameObject.EnvironmentGame.IsWalkable || t.HasUnit())
                        {
                            continue;
                        }
                    }

                    double gCost = current.GCost + GetCost(current.Vector2, t.Vector2);
                    double hCost = GetCost(t.Vector2, goal.Vector2);

                    var node = new Node(t.Vector2, t, current, gCost, hCost);

                    if (closedList.Contains(node))
                    {
                        continue;
                    }
                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }

                    maxValueCounter++;
                    if (maxValueCounter > 100) return null;
                }
            }

            // No paths found
            return null;
        }

        public double GetCost(Vector2 a, Vector2 b)
        {
            double x = (a.x - b.x);
            double y = (a.y - b.y);
            x *= x;
            y *= y;

            return Math.Sqrt(x + y);
        }

        public class CompareNodes : Comparer<Node>
        {
            public override int Compare(Node n0, Node n1)
            {
                if (n1.FCost < n0.FCost)
                {
                    return 1;
                }
                if (n1.FCost > n0.FCost)
                {
                    return -1;
                }

                return 0;
            }
        }
    }
}