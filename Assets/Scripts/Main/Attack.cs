using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attack
{
    // Show attackhighlight, gets called in the ShowMovement method in Movement class
    // because when unit clicks on unit we also need to show attackhighlight.
    public void ShowAttack(Tile tile)
    {
        if (tile.unitGameObject != null)
        {
            if (GameManager.Instance.CurrentPlayer != GameManager.Instance.GetPlayer(tile.unitGameObject.index))
            {
                // Show enemy highlight
                GameObject attackHighlightGO = tile.transform.FindChild("highlight_attack").gameObject;
                
                attackHighlightGO.SetActive(true);
                GameManager.Instance.attackHighLightObjects.Add(attackHighlightGO);
            }
        }
    }
}

