using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attack
{    
    public Attack()
    {
        EventHandler.register<OnHighlightClick>(BattlePreparation);
    }

    /// <summary>
    /// Show the attack highlights for the specified unit with the specified range.
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public int ShowAttackHighlights(UnitGameObject unit, int range, Movement _movement)
    {
        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in GameManager.Instance.GetAllTilesWithinRange(unit.tile.Coordinate, range))
        {
            foreach (KeyValuePair<int, Tile> tile in item.Value)
            {
                if (tile.Value.HasUnit() && tile.Value.unitGameObject.index != unit.index)
                {
                    // If unit is an archer we don't need to calculate paths because archer can shoot over units, water etc.
                    if (!tile.Value.unitGameObject.unitGame.CanAttackAfterMove)
                    {
                        tile.Value.highlight.ChangeHighlight(HighlightTypes.highlight_attack);
                        GameManager.Instance.highlight.highlightObjects.Add(tile.Value.highlight);
                    }
                    else
                    {
                        List<Node> path = _movement.CalculateShortestPath(unit.tile, tile.Value, true);

                        if (path != null && path.Count <= unit.unitGame.GetAttackMoveRange)
                        {
                            tile.Value.highlight.ChangeHighlight(HighlightTypes.highlight_attack);
                            GameManager.Instance.highlight.highlightObjects.Add(tile.Value.highlight);
                        }
                    }                   
                }
            }
        }
        int count = GameManager.Instance.highlight.highlightObjects.Count;
        GameManager.Instance.highlight.isHighlightOn = count > 0;
        return count;
    }

    /// <summary>
    /// Get called whenever an OnHighlightClick is fired. If it is possible it will attack an enemy unit.
    /// </summary>
    /// <param name="evt"></param>
    public void BattlePreparation(OnHighlightClick evt)
    {
        if(evt.highlight != null)
        {
            HighlightObject highlight = evt.highlight;
            if (GameManager.Instance.highlight.isHighlightOn && !GameManager.Instance.highlight._movement.needsMoving && highlight.highlightTypeActive == HighlightTypes.highlight_attack)
            {
                UnitGameObject attackingUnit = GameManager.Instance.highlight._unitSelected;
                UnitGameObject defendingUnit = highlight.tile.unitGameObject;
                if (!attackingUnit.unitGame.hasAttacked)
                {
                    if (!attackingUnit.unitGame.hasMoved || (attackingUnit.unitGame.hasMoved && attackingUnit.unitGame.CanAttackAfterMove))
                    {
                        BattleSimulation(attackingUnit, defendingUnit);
                    }
                }
            }
        }
    }

    /// <summary>
    /// The method which does all the complicated battle simulation. From calculating damage to dealing damage and so forth.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    private void BattleSimulation(UnitGameObject attacker, UnitGameObject defender)
    {
        // NOTE!!!!!!!
        // Because are sprites are a certain size. The location of the tiles is always in increments of 2 or negative 2. So tile(1,1) is on 0,0,0. tile(2,1) is on 2,0,0. 
        // tile(1,2) is on 0,-2,0 etc.
        // Because of these increments, 2 unity meters equals to 1 range. When we say attackrange of 1 you can attack units directly beside you.
        // So thats why we devide the distance between the two units by 2 in order to get the correct distance.
        // If we later have different sprite sizes and/or different increments this needs to change.
        // This works for all ranges.
        float distanceBetweenUnits = Vector2.Distance(attacker.transform.position, defender.transform.position) / 2;
        if (distanceBetweenUnits <= attacker.unitGame.attackRange)
        {
            attacker.unitGame.hasMoved = true;
            attacker.unitGame.hasAttacked = true;
            defender.unitGame.DecreaseHealth(3);
            attacker.unitGame.PlaySound(UnitSoundType.Attack);
            GameManager.Instance.highlight.ClearNewHighlights();
        }
    }
}