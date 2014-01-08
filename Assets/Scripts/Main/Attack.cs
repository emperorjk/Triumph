using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attack
{
    private RaycastHit _touchBox;
    private List<GameObject> closeAttackHighlights;

    public void ShowAttackHighlight(Dictionary<int, Dictionary<int, Tile>> attackHighlightList)
    {
        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in attackHighlightList)
        {
            foreach (KeyValuePair<int, Tile> tile in item.Value)
            {
                if (tile.Value.unitGameObject != null && GameManager.Instance.CurrentPlayer.index != tile.Value.unitGameObject.index)
                {
                    tile.Value.HighlightAttack.SetActive(true);
                    GameManager.Instance.attackHighLightObjects.Add(tile.Value.HighlightAttack);
                    break;
                }
            }
        }
    }

    public void CollisionAttackRange(Tile tile)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _touchBox))
        {
            foreach (GameObject attackHighlight in GameManager.Instance.attackHighLightObjects)
            {
                if (_touchBox.collider == attackHighlight.collider)
                {
                    // Only range people can attack with tiles between them
                    if (!tile.unitGameObject.unitGame.CanAttackAfterMove)
                    {
                        Tile enemyUnitTile = attackHighlight.transform.parent.GetComponent<Tile>();
                        enemyUnitTile.unitGameObject.unitGame.DecreaseHealth((int)tile.unitGameObject.unitGame.damage * 5);
                        tile.unitGameObject.unitGame.hasAttacked = true;
                        tile.unitGameObject.renderer.material.color = Color.gray;
                    }
                }
            }
        }
    }

    public void CollisionAttackMelee(Dictionary<int, Dictionary<int, Tile>> attackHighlightList, Tile tile)
    {
        closeAttackHighlights = new List<GameObject>();

        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in attackHighlightList)
        {
            foreach (KeyValuePair<int, Tile> t in item.Value)
            {
                if (t.Value.unitGameObject != null)
                {
                    closeAttackHighlights.Add(t.Value.HighlightAttack); 
                }
            }
        }
        AttackNearbyEnemies(tile);
    }

    public void AttackNearbyEnemies(Tile tile)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _touchBox))
        {
            foreach (GameObject attackHighlight in closeAttackHighlights)
            {
                if (_touchBox.collider == attackHighlight.collider)
                {
                    Tile enemyUnitTile = attackHighlight.transform.parent.GetComponent<Tile>();
                    enemyUnitTile.unitGameObject.unitGame.DecreaseHealth((int)tile.unitGameObject.unitGame.damage * 5);
                    tile.unitGameObject.unitGame.hasAttacked = true;
                    tile.unitGameObject.transform.gameObject.renderer.material.color = Color.gray;
                    GameManager.Instance.UnitCanAttack = false;
                }
            }
        }
    }
}