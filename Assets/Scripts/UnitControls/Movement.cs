using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Movement
{
    private RaycastHit _touchBox;
    private float duration = 2f;

    public void Move(GameObject LastClickedUnitGO, Tile LastClickedUnitTile, Vector2 startPosition, Vector2 destionationLocation, Attack attack, Dictionary<int, Dictionary<int, Tile>> attackHighlightList)
    {
        LastClickedUnitGO.transform.position = Vector2.Lerp(startPosition, destionationLocation, (Time.time - GameManager.Instance.StartTime) / duration);

        if ((Time.time - GameManager.Instance.StartTime) / duration >= 1f)
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

            if (LastClickedUnitTile.unitGameObject.unitGame.CanAttackAfterMove)
            {
                attackHighlightList = GameManager.Instance.GetAllTilesWithinRange(LastClickedUnitTile.unitGameObject.tile.Coordinate, LastClickedUnitTile.unitGameObject.unitGame.attackRange);
                
                if (attack.ShowAttackHighlight(attackHighlightList))
                {
                    LastClickedUnitGO.renderer.material.color = Color.white;
                    GameManager.Instance.UnitCanAttack = true;
                }
            }

            LastClickedUnitGO = null;
            LastClickedUnitTile.unitGameObject = null;
            LastClickedUnitTile = null;
        }
    }
}