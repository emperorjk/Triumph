using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Movement
{
    private Dictionary<int, Dictionary<int, Tile>> list;
    private Attack attack = new Attack();

    private RaycastHit _touchBox;
    private GameObject LastClickedUnitGO;
    private Tile LastClickedUnitTile;
    private Vector2 startPosition;
    private Vector2 destionationLocation;

    private float startTime;
    private float duration = 2f;

    public void ShowMovementHighLight(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out _touchBox))
        {
            foreach (UnitBase b in player.ownedUnits)
            {
                if (_touchBox.collider == b.unitGameObject.collider)
                {
                    if (!b.hasMoved)
                    {
                        LastClickedUnitTile = b.unitGameObject.tile;
                        LastClickedUnitGO = b.unitGameObject.gameObject;
                        ShowMovement(b.unitGameObject);
                        GameManager.Instance.IsHightlightOn = true;
                    }
                }
            }
        }
    }

    public void ShowMovement(UnitGameObject unit)
    {
        list = GameManager.Instance.GetAllTilesWithinRange(unit.tile.coordinate, unit.unitGame.moveRange);

        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in list)
        {
            foreach (KeyValuePair<int, Tile> tile in item.Value)
            {
                if (!tile.Value.HasUnit() && tile.Value.environmentGameObject.environmentGame.IsWalkable)
                {
                    GameObject highlightGO = tile.Value.transform.FindChild("highlight_move").gameObject;
                    highlightGO.SetActive(true);
                    GameManager.Instance.highLightObjects.Add(highlightGO);
                }
               attack.ShowAttack(tile.Value);
            }
        }
    }

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

                    // Set the current Tile to null
                    LastClickedUnitTile.unitGameObject = null;

                    // Set the destionation Tile to the ClickedUnit
                    Tile destinationTile = highlight.transform.parent.gameObject.GetComponent<Tile>();
                    destinationTile.unitGameObject = LastClickedUnitGO.GetComponent<UnitGameObject>();
                    destinationTile.unitGameObject.tile = destinationTile;

                    // Start moving in update loop
                    GameManager.Instance.NeedMoving = true;
                }
            }
        }

        DeactivateHighLights();

        // recreate list, otherwise list gets filled with duplicates
        GameManager.Instance.highLightObjects.Clear();

        // disable current highlight
        GameManager.Instance.IsHightlightOn = false;

        // Call this method because we want to activate the highlight if user clicks on another unit
        ShowMovementHighLight(GameManager.Instance.CurrentPlayer);
    }

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

            // set color to gray so player knows unit has             
            LastClickedUnitGO.renderer.material.color = Color.gray;
            GameManager.Instance.NeedMoving = false;
            LastClickedUnitGO = null;
            LastClickedUnitTile = null;
        }
    }
}