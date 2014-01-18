using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Highlight
{
    private GameManager _manager;
    private Movement _movement;
    private Attack _attack;

    private Dictionary<int, Dictionary<int, Tile>> movementList;
    private Dictionary<int, Dictionary<int, Tile>> attackHighlightList;
    private List<Node> nodeList;

    private Tile LastClickedUnitTile;
    private Vector2 startPosition;
    private Vector2 destionationLocation;
    private RaycastHit _touchBox;

    public Highlight()
    {
        _manager = GameManager.Instance;
        _movement = new Movement();
        _attack = new Attack();
    }

    public void HandleHighlightInput()
    {
        if (Input.GetMouseButtonDown(0) && !_manager.NeedMoving)
        {
            // If highlight is turned on we want to check if user clicked on a highlight or not
            if (!_manager.IsHightlightOn)
            {
                ClickedUnitCollider(_manager.CurrentPlayer);
            }
            else if (_manager.IsHightlightOn)
            {
                CollisionWithHighlight();
            }

            if (_manager.UnitCanAttack)
            {
                _manager.ClearHighlight();
                _manager.UnitCanAttack = false;
            }
        }

        // If user clicked on a highlight we want to call the Move method from this update loop
        if (_manager.NeedMoving)
        {
           // _movement.Move(LastClickedUnitTile, startPosition, destionationLocation, _attack, attackHighlightList);
            _movement.Move(nodeList, LastClickedUnitTile, startPosition);
           
        }
    }

    public void ClickedUnitCollider(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _touchBox))
        {
            foreach (UnitBase b in player.ownedUnits)
            {
                if (_touchBox.collider == b.unitGameObject.collider)
                {
                    if (!b.hasAttacked && (!b.hasMoved || b.CanAttackAfterMove)) 
                    {
                        LastClickedUnitTile = b.unitGameObject.tile;
                        ShowHighlights(b.unitGameObject);
                        _manager.IsHightlightOn = true;
                        break;
                    }
                }
            }
        }
    }

    public void ShowHighlights(UnitGameObject unit)
    {
        LastClickedUnitTile.unitGameObject.unitGame.PlaySound(UnitSoundType.Select);

        if (!unit.unitGame.hasMoved)
        {
            movementList = _manager.GetAllTilesWithinRange(unit.tile.Coordinate, unit.unitGame.moveRange);

            attackHighlightList = _manager.GetAllTilesWithinRange(unit.tile.Coordinate, unit.unitGame.GetAttackRange);        
            foreach (KeyValuePair<int, Dictionary<int, Tile>> item in movementList)
            {
                foreach (KeyValuePair<int, Tile> tile in item.Value)
                {
                    if (!tile.Value.HasUnit() && tile.Value.environmentGameObject.environmentGame.IsWalkable)
                    {
                        tile.Value.HighlightMove.SetActive(true);
                        _manager.highLightObjects.Add(tile.Value.HighlightMove);
                    }
                }
            }
            _attack.ShowAttackHighlight(attackHighlightList);
        }
        else if(unit.unitGame.CanAttackAfterMove)
        {
            attackHighlightList = _manager.GetAllTilesWithinRange(unit.tile.Coordinate, unit.unitGame.attackRange);        
            _attack.ShowAttackHighlight(attackHighlightList);
        }        
    }

    public void CollisionWithHighlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _touchBox))
        {
            foreach (GameObject highlight in _manager.highLightObjects)
            {
                if (_touchBox.collider == highlight.collider)
                {
                    _manager.StartTime = Time.time;

                    // Set the start and destionation position
                    startPosition = new Vector2(LastClickedUnitTile.transform.position.x, LastClickedUnitTile.transform.position.y);
                    destionationLocation = highlight.transform.position;

                    LastClickedUnitTile.unitGameObject.unitGame.hasMoved = true;

                    // Set the destionation Tile
                    Tile destinationTile = highlight.transform.parent.gameObject.GetComponent<Tile>();

                    // calculate shortest path
                    nodeList = _movement.CalculateShortestPath(LastClickedUnitTile, destinationTile);

                    destinationTile.unitGameObject = LastClickedUnitTile.unitGameObject;
                    destinationTile.unitGameObject.tile = destinationTile;

                    // Start moving in update loop
                    _manager.NeedMoving = true;

                    LastClickedUnitTile.unitGameObject.unitGame.PlaySound(UnitSoundType.Move);

                    break;
                }
            }
        }

        _attack.CollisionAttackRange(LastClickedUnitTile);

        if (!LastClickedUnitTile.unitGameObject.unitGame.hasAttacked)
        {
            _attack.CollisionAttackMelee(_manager.GetAllTilesWithinRange(LastClickedUnitTile.unitGameObject.tile.Coordinate, LastClickedUnitTile.unitGameObject.unitGame.attackRange), LastClickedUnitTile);
        }

        _manager.ClearHighlight();

        // Call this method because we want to activate the highlight if user clicks on another unit
        ClickedUnitCollider(GameManager.Instance.CurrentPlayer);
    }
}