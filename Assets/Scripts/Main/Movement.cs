using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Movement
{
    private Dictionary<int, Dictionary<int, Tile>> movementList;
    private Dictionary<int, Dictionary<int, Tile>> attackHighlightList;
    private Attack attack = new Attack();

    private RaycastHit _touchBox;
    private GameObject LastClickedUnitGO;
    private Tile LastClickedUnitTile;
    private UnitGameObject LastClickedUnitTileAttackNearby;
    private Vector2 startPosition;
    private Vector2 destionationLocation;

    private float startTime;
    private float duration = 2f;

    /// <summary>
    /// Loops through all units from the player and checks if they can move.
    /// If unit can move we call ShowHighlight method for showing highlights.
    /// </summary>
    /// <param name="player"></param>
    public void ShowMovementUnit(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out _touchBox))
        {
            foreach (UnitBase b in player.ownedUnits)
            {
                if (_touchBox.collider == b.unitGameObject.collider)
                {
                    if (!b.hasMoved && !b.hasAttacked)
                    {
                        LastClickedUnitTile = b.unitGameObject.tile;
                        LastClickedUnitTileAttackNearby = LastClickedUnitTile.unitGameObject;
                        LastClickedUnitGO = b.unitGameObject.gameObject;
                        ShowHighlights(b.unitGameObject);
                        GameManager.Instance.IsHightlightOn = true;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Method for showing / enable highlights. Attack and movement highlights are created first.
    /// We also call the ShowAttack method for showing attack highlights.
    /// </summary>
    /// <param name="unit"></param>
    public void ShowHighlights(UnitGameObject unit)
    {
        movementList = GameManager.Instance.GetAllTilesWithinRange(unit.tile.Coordinate, unit.unitGame.moveRange);
        attackHighlightList = GameManager.Instance.GetAllTilesWithinRange(unit.tile.Coordinate, unit.unitGame.GetAttackRange);

        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in movementList)
        {
            foreach (KeyValuePair<int, Tile> tile in item.Value)
            {
                if (!tile.Value.HasUnit() && tile.Value.environmentGameObject.environmentGame.IsWalkable)
                {
                    tile.Value.HighlightMove.SetActive(true);
                    GameManager.Instance.highLightObjects.Add(tile.Value.HighlightMove);
                }
            }
        }
        attack.ShowAttackHighlight(attackHighlightList);
    }

    /// <summary>
    /// Check if user has clicked on a highlight collider, if user has clicked on highlight
    /// unit will move.
    /// </summary>
    public void CollisionWithHighlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _touchBox))
        {
            foreach (GameObject highlight in GameManager.Instance.highLightObjects)
            {
                if (_touchBox.collider == highlight.collider)
                {
                    startTime = Time.time;

                    // Set the start and destionation position
                    startPosition = new Vector2(LastClickedUnitTile.transform.position.x, LastClickedUnitTile.transform.position.y);
                    destionationLocation = highlight.transform.position;

                    LastClickedUnitTile.unitGameObject.unitGame.hasMoved = true;

                    // Set the destionation Tile to the ClickedUnit
                    Tile destinationTile = highlight.transform.parent.gameObject.GetComponent<Tile>();
                    destinationTile.unitGameObject = LastClickedUnitGO.GetComponent<UnitGameObject>();
                    destinationTile.unitGameObject.tile = destinationTile;

                    // Start moving in update loop
                    GameManager.Instance.NeedMoving = true;

                    break;
                }
            }
        }
      
        attack.CollisionAttackRange(LastClickedUnitTile);

        if (!LastClickedUnitTile.unitGameObject.unitGame.hasAttacked)
        {
            attack.CollisionAttackMelee(GameManager.Instance.GetAllTilesWithinRange(LastClickedUnitTile.unitGameObject.tile.Coordinate, LastClickedUnitTile.unitGameObject.unitGame.attackRange), LastClickedUnitTile);
        }

        DeactivateHighLights();

        // Call this method because we want to activate the highlight if user clicks on another unit
        ShowMovementUnit(GameManager.Instance.CurrentPlayer);
    }

    public void Move()
    {
        LastClickedUnitGO.transform.position = Vector2.Lerp(startPosition, destionationLocation, (Time.time - startTime) / duration);

        if ((Time.time - startTime) / duration >= 1f)
        {
            Tile tileUnitMovedTo = LastClickedUnitGO.GetComponent<UnitGameObject>().tile;
            if (tileUnitMovedTo.HasBuilding())
            {
                GameManager.Instance.CaptureBuildings.AddBuildingToCaptureList(tileUnitMovedTo.buildingGameObject.buildingGame);
            }

            // Set the unit transform.parent to the new tile which is has moved to. This way the position resets to 0,0,0 of the unit and it is always perfectly 
            // placed onto the tile which it is on. It also changes the objects in the hierarchie window under the new tile object.
            LastClickedUnitGO.transform.parent = tileUnitMovedTo.transform;
            GameManager.Instance.NeedMoving = false;
            
            if(LastClickedUnitTile.unitGameObject.unitGame.hasMoved)
            {
                LastClickedUnitGO.renderer.material.color = Color.gray;
            }

            if (LastClickedUnitTile.unitGameObject.unitGame.CanAttackAfterMove )
            {
                attackHighlightList = GameManager.Instance.GetAllTilesWithinRange(LastClickedUnitTile.unitGameObject.tile.Coordinate, LastClickedUnitTile.unitGameObject.unitGame.attackRange);
                attack.ShowAttackHighlight(attackHighlightList);
                GameManager.Instance.UnitCanAttack = true;
            }

            LastClickedUnitGO = null;
            LastClickedUnitTile.unitGameObject = null;
            LastClickedUnitTile = null;
        }
    }

    public void AttackCloseEnemy()
    {
        attackHighlightList = GameManager.Instance.GetAllTilesWithinRange(LastClickedUnitTileAttackNearby.tile.Coordinate, LastClickedUnitTileAttackNearby.unitGame.attackRange);
        attack.CollisionAttackMelee(attackHighlightList, LastClickedUnitTileAttackNearby.transform.parent.gameObject.GetComponent<Tile>());
        DeactivateHighLights();
    }

    /// <summary>
    /// Deactivate highlight and set them to non-active. Clear the two lists with highlight.
    /// </summary>
    private void DeactivateHighLights()
    {
        foreach (GameObject highlights in GameManager.Instance.highLightObjects)
        {
            highlights.SetActive(false);
        }

        foreach (GameObject attackHighlight in GameManager.Instance.attackHighLightObjects)
        {
            attackHighlight.SetActive(false);
        }

        // Clear list, otherwise list gets filled with duplicates
        GameManager.Instance.highLightObjects.Clear();
        GameManager.Instance.attackHighLightObjects.Clear();

        GameManager.Instance.IsHightlightOn = false;
    }
}