using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.UnitActions
{
    public class Node
    {
        public Vector2 Vector2 { get; private set; }
        public Tile Tile { get; private set; }
        public Node Parent { get; private set; }
        public double FCost { get; private set; }
        public double GCost { get; private set; }
        public double HCost { get; private set; }

        public Node(Vector2 vector2, Tile tile, Node parent, double gCost, double hCost)
        {
            Vector2 = vector2;
            Tile = tile;
            Parent = parent;
            GCost = gCost;
            HCost = hCost;
            FCost = GCost + HCost;
        }
    }
}