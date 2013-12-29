using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Movement
{
    private Dictionary<int, Dictionary<int, Tile>> list;
    private List<GameObject> highLightObjects = new List<GameObject>();

    private RaycastHit _touchBox;
    private Vector2 destionationLocation;
    private Vector2 startPosition;

    private bool moveUnit = false;
    private float startTime;
    private float duration = 2f;

    public void ShowMovement(UnitGameObject unit)
    {
        list = GameManager.Instance.GetAllTilesWithinRange(unit.tile.coordinate, unit.unitGame.moveRange);

        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in list)
        {
            foreach (KeyValuePair<int, Tile> tile in item.Value)
            {
                if (tile.Value.CanUnitBePlacedOn())
                {
                    GameObject highlightGO = tile.Value.transform.FindChild("Highlight").gameObject;
                    highlightGO.SetActive(true);
                    highLightObjects.Add(highlightGO);
                }
            }
        }
    }

    public void ShowMovementHighLight(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        foreach (UnitBase b in player.ownedUnits)
        {
            if (Physics.Raycast(ray, out _touchBox))
            {
                if (_touchBox.collider == b.unitGameObject.collider)
                {
                    if (!b.hasMoved)
                    {
                        GameManager.Instance.LastClickedUnitTile = b.unitGameObject.tile;
                        GameManager.Instance.LastClickedUnitGO = b.unitGameObject.gameObject;

                        ShowMovement(b.unitGameObject);
                        GameManager.Instance.IsHightlightOn = true;
                    }
                }
            }
        }
    }

    public void CollisionWithHightlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _touchBox))
        {
            foreach (GameObject highlight in highLightObjects)
            {
                if (_touchBox.collider == highlight.collider)
                {
                    destionationLocation = highlight.transform.position;
                    MoveUnit();
                }
            }
        }

        foreach (GameObject highlights in highLightObjects)
        {
            highlights.SetActive(false);
        }
        // recreate list, otherwise list gets filled with duplicates
        highLightObjects = new List<GameObject>();

        // disable current highlight
        GameManager.Instance.IsHightlightOn = false;

        // Call this method because we want to activate the highlight if user clicks on another unit
        ShowMovementHighLight(GameManager.Instance.CurrentPlayer);
    }

    public void MoveUnit()
    {
        startTime = Time.time;
        //startPosition = new Vector2(LastClickedUnit.transform.position.x, LastClickedUnit.transform.position.y);

        //LastClickedUnit.ColumnId += 2;
        //LastClickedUnit.transform. = highlight.transform.localPosition;

        if (moveUnit)
        {
            //LastClickedUnitGameObject.transform.position = Vector2.Lerp(startPosition, destionationLocation, (Time.time - startTime) / duration);
            // stop this after movement is done
        }
    }
}

