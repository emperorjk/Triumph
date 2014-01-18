using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Node
{
    public Vector2 vector2;
    public Tile tile;
    public Node parent;
    public double fCost, gCost, hCost;

    public Node(Vector2 vector2, Tile tile, Node parent, double gCost, double hCost)
    {
        this.vector2 = vector2;
        this.tile = tile;
        this.parent = parent;
        this.gCost = gCost;
        this.hCost = hCost;
        this.fCost = this.gCost + this.hCost;
    }
}