using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Movement
{
    private RaycastHit _touchBox;
    private float duration = 2f;

    public void Move(Tile LastClickedUnitTile, Vector2 startPosition, Vector2 destionationLocation, Attack attack, Dictionary<int, Dictionary<int, Tile>> attackHighlightList)
    {
        LastClickedUnitTile.unitGameObject.gameObject.transform.position = Vector2.Lerp(startPosition, destionationLocation, (Time.time - GameManager.Instance.StartTime) / duration);

        if ((Time.time - GameManager.Instance.StartTime) / duration >= 1f)
        {
            // Stop calling this method
            GameManager.Instance.NeedMoving = false;
            Tile destionationTile = LastClickedUnitTile.unitGameObject.gameObject.GetComponent<UnitGameObject>().tile;

            if (destionationTile.HasBuilding())
            {
                GameManager.Instance.CaptureBuildings.AddBuildingToCaptureList(destionationTile.buildingGameObject.buildingGame);
            }

            // Set the unit transform.parent to the new tile which is has moved to. This way the position resets to 0,0,0 of the unit and it is always perfectly 
            // placed onto the tile which it is on. It also changes the objects in the hierarchie window under the new tile object.
            LastClickedUnitTile.unitGameObject.gameObject.transform.parent = destionationTile.transform;
            
            if(LastClickedUnitTile.unitGameObject.unitGame.hasMoved)
            {
                LastClickedUnitTile.unitGameObject.gameObject.renderer.material.color = Color.gray;
            }

            if (LastClickedUnitTile.unitGameObject.unitGame.CanAttackAfterMove)
            {
                attackHighlightList = GameManager.Instance.GetAllTilesWithinRange(LastClickedUnitTile.unitGameObject.tile.Coordinate, LastClickedUnitTile.unitGameObject.unitGame.attackRange);
                
                // if this unit can attack after movement and has an enemy standing next to him we know this UnitCanAttack
                if (attack.ShowAttackHighlight(attackHighlightList))
                {
                    LastClickedUnitTile.unitGameObject.gameObject.renderer.material.color = Color.white;
                    GameManager.Instance.UnitCanAttack = true;
                }
            }

            LastClickedUnitTile.unitGameObject = null;
            LastClickedUnitTile = null;
        }
    }
}