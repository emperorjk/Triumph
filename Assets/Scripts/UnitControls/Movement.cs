using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float StartTimeMoving { get; set; }
    public bool NeedsMoving { get; set; }
    public List<Node> nodeList;

    private CompareNodes compare = new CompareNodes();
    private float movingDuration = 1f;

    void Update()
    {
        if (NeedsMoving && nodeList != null)
        {
            Moving(GameManager.Instance.Highlight.UnitSelected);
        }
    }

    public void Moving(UnitGameObject unitMoving)
    {
        unitMoving.transform.position = Vector2.Lerp(unitMoving.Tile.Vector2, nodeList.Last().tile.Vector2, GetTimePassed());
        
        if (GetTimePassed() >= 1f)
        {
            // Show fow for the unit.
            GameManager.Instance.Fow.ShowFowWithinLineOfSight(unitMoving.index);
            // Remove the references from the old tile.
            unitMoving.Tile.unitGameObject = null;
            unitMoving.Tile = null;
            // Remove the last tile from the list.
            Tile newPosition = nodeList.Last().tile;
            nodeList.Remove(nodeList.Last());
            // Assign the references using the new tile.
            newPosition.unitGameObject = unitMoving;
            unitMoving.Tile = newPosition;
            unitMoving.Tile.Vector2 = newPosition.Vector2;
            // Set the parent and position of the unit to the new tile.
            unitMoving.transform.parent = newPosition.transform;
            unitMoving.transform.position = newPosition.transform.position;
            // Hide the fow for the unit. It will use the new tile location.
            GameManager.Instance.Fow.HideFowWithinLineOfSight(unitMoving.index);
            StartTimeMoving = Time.time;
        }

        if(nodeList.Count <= 0)
        {
            GameManager.Instance.Highlight.ClearHighlights();
            Tile endDestinationTile = unitMoving.Tile;

            if (endDestinationTile.HasLoot())
            {
                endDestinationTile.Loot.PickUpLoot(GameManager.Instance.CurrentPlayer);
            } 
           
            if (endDestinationTile.HasBuilding())
            {
                GameManager.Instance.CaptureBuildings.AddBuildingToCaptureList(endDestinationTile.buildingGameObject.buildingGame);
            }

            if(unitMoving.UnitGame.CanAttackAfterMove && GameManager.Instance.Attack.ShowAttackHighlights(unitMoving, unitMoving.UnitGame.AttackRange) > 0)
            {
                unitMoving.UnitGame.hasMoved = true;
            }
            else
            {
                unitMoving.UnitGame.hasMoved = true;
                unitMoving.UnitGame.hasAttacked = true;
            }
            NeedsMoving = false;
            unitMoving.UnitGame.UpdateUnitColor();
        }
    }

    public void FacingDirectionMovement(UnitGameObject moveUnit, Tile destination)
    {
        Vector3 direction = moveUnit.transform.position - destination.transform.position;

        Quaternion quaternion = new Quaternion(0, (direction.x > 0 ? 180 : 0), 0, 0);
        moveUnit.transform.rotation = quaternion;
    }

    private float GetTimePassed()
    {
        return (Time.time - StartTimeMoving) / movingDuration;
    }

    /// <summary>
    /// Calculates the shortest path with the A* search algorithm.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    public List<Node> CalculateShortestPath(Tile start, Tile goal, bool attackCalculate)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        Node current = new Node(start.Vector2, start, null, 0, 0);
        int maxValueCounter = 0;

        openList.Add(current);
        while(openList.Count > 0)
        {
            openList.Sort(compare);
            current = openList[0];

            if (current.tile.Equals(goal))
            {
                List<Node> path = new List<Node>();

                while (current.parent != null)
                {
                    path.Add(current);
                    current = current.parent;
                }
                openList.Clear();
                closedList.Clear();

                return path;
            }
            
            openList.Remove(current);
            closedList.Add(current);
            for (int i = 0; i < 9; i++)
            {
                int x = (i % 3) - 1;
                int y = (i / 3) - 1;

                // This line prevents diagonal movement
                if (!((x == 0 && y != 0) || (x != 0 && y == 0)))
                {
                    continue;
                }

                Tile t = TileHelper.GetTile(new TileCoordinates(x + current.tile.ColumnId, y + current.tile.RowId));

                if (t == null)
                {
                    continue;
                }
                else if (attackCalculate)
                {
                    // if T is not equal to goal we want to check if isWalkable and HasUnit. Otherwise we cannot move to goal because that one has an unit.
                    if (!t.Equals(goal))
                    {
                        if (!t.environmentGameObject.environmentGame.IsWalkable || t.HasUnit())
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    if (!t.environmentGameObject.environmentGame.IsWalkable || t.HasUnit())
                    {
                        continue;
                    }
                }

                double gCost = current.gCost + GetCost(current.vector2, t.Vector2);
                double hCost = GetCost(t.Vector2, goal.Vector2);

                Node node = new Node(t.Vector2, t, current, gCost, hCost);

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
            if (n1.fCost < n0.fCost)
            {
                return 1;
            }
            else if (n1.fCost > n0.fCost)
            {
                return -1;
            }

            return 0;
        }
    }
}