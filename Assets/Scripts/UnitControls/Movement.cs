using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Movement
{
    private RaycastHit _touchBox;
    private CompareNodes compare = new CompareNodes();
    private List<Node> nodeList;
    private float duration = 1f;

    public void Move(Tile LastClickedUnitTile, Attack attack, Dictionary<int, Dictionary<int, Tile>> attackHighlightList)
    {
        LastClickedUnitTile.unitGameObject.gameObject.transform.position = Vector2.Lerp(LastClickedUnitTile.unitGameObject.gameObject.transform.position, nodeList.Last().tile.Vector2, (Time.time - GameManager.Instance.StartTime) / duration);

        if ((Time.time - GameManager.Instance.StartTime) / duration >= 1f)
        {
            nodeList.Remove(nodeList.Last());
            GameManager.Instance.StartTime = Time.time;
        }
        if (nodeList.Count <= 0)
        {
            Tile destionationTile = LastClickedUnitTile.unitGameObject.gameObject.GetComponent<UnitGameObject>().tile;

            if (destionationTile.HasBuilding())
            {
                GameManager.Instance.CaptureBuildings.AddBuildingToCaptureList(destionationTile.buildingGameObject.buildingGame);
            }

            // Set the unit transform.parent to the new tile which is has moved to. This way the position resets to 0,0,0 of the unit and it is always perfectly 
            // placed onto the tile which it is on. It also changes the objects in the hierarchie window under the new tile object.
            LastClickedUnitTile.unitGameObject.gameObject.transform.parent = destionationTile.transform;

            if (LastClickedUnitTile.unitGameObject.unitGame.hasMoved)
            {
                if (LastClickedUnitTile.unitGameObject.unitGame.CanAttackAfterMove)
                {
                    attackHighlightList = GameManager.Instance.GetAllTilesWithinRange(LastClickedUnitTile.unitGameObject.tile.Coordinate, LastClickedUnitTile.unitGameObject.unitGame.attackRange);

                    // if this unit can attack after movement and has an enemy standing next to him we know this UnitCanAttack
                    if (attack.ShowAttackHighlight(attackHighlightList) > 0)
                    {
                        LastClickedUnitTile.unitGameObject.gameObject.renderer.material.color = Color.white;
                        GameManager.Instance.UnitCanAttack = true;
                    }
                    else
                    {
                        LastClickedUnitTile.unitGameObject.unitGame.hasMoved = true;
                        LastClickedUnitTile.unitGameObject.unitGame.hasAttacked = true;
                        LastClickedUnitTile.unitGameObject.gameObject.renderer.material.color = Color.gray;
                        LastClickedUnitTile.unitGameObject = null;
                        LastClickedUnitTile = null;
                    }
                }
                else 
                {
                    LastClickedUnitTile.unitGameObject.gameObject.renderer.material.color = Color.gray;
                    LastClickedUnitTile.unitGameObject = null;
                    LastClickedUnitTile = null;
                }
            }          

            GameManager.Instance.NeedMoving = false;
        }
    }

    /// <summary>
    /// Calculates the shortest path with the A* search algorithm.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    public List<Node> CalculateShortestPath(Tile start, Tile goal)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        Node current = new Node(start.Vector2, start, null, 0, 0);

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

                nodeList = path;
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

                Tile t = GameManager.Instance.GetTile(new TileCoordinates(x + current.tile.ColumnId, y + current.tile.RowId));

                if (t == null) 
                {
                    continue;
                }
                else if (!t.environmentGameObject.environmentGame.IsWalkable || t.HasUnit())
                {
                    continue;
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