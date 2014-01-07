using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attack
{
    private RaycastHit _touchBox;

    // Show attackhighlight, gets called in the ShowMovement method in Movement class
    // because when unit clicks on unit we also need to show attackhighlight.
    public void ShowAttack(Dictionary<int, Dictionary<int, Tile>> attackHighlightList)
    {
        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in attackHighlightList)
        {
            foreach (KeyValuePair<int, Tile> tile in item.Value)
            {
                    if (tile.Value.unitGameObject != null && GameManager.Instance.CurrentPlayer.index != tile.Value.unitGameObject.index)
                    {
                        GameObject attackHighlightGO = tile.Value.transform.FindChild("highlight_attack").gameObject;
                        attackHighlightGO.SetActive(true);
                        GameManager.Instance.attackHighLightObjects.Add(attackHighlightGO);
                        break;
                    }
            }
        }
    }

    public bool CollisionWithAttackHighlight(Tile tile)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _touchBox))
        {
            foreach (GameObject attackHighlight in GameManager.Instance.attackHighLightObjects)
            {
                if (_touchBox.collider == attackHighlight.collider)
                {
                    // Als range persoon aanvallen, anders niet
                    if (!tile.unitGameObject.unitGame.CanAttackAfterMove)
                    {
                        Tile enemyUnitTile = attackHighlight.transform.parent.GetComponent<Tile>();
                        // method decrease health, increase health, isDead
                        enemyUnitTile.unitGameObject.unitGame.DecreaseHealth((int)tile.unitGameObject.unitGame.damage * 4);
                        // this is not clean. Is nicer when there is a isDead method to check if the unit has died. Then call the Die method.
                        if (enemyUnitTile.unitGameObject != null)
                        {
                            enemyUnitTile.unitGameObject.UpdateCapturePointsText();
                        }
                        //Debug.Log(enemyUnitTile.unitGameObject.unitGame.health);


                        return true;
                    }
                }
            }
        }
        return false;
    }
}