using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attack
{
    public AnimationInfo animInfo;

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
        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in TileHelper.GetAllTilesWithinRange(unit.tile.Coordinate, range))
        {
            foreach (KeyValuePair<int, Tile> tile in item.Value)
            {
                if (tile.Value.HasUnit() && tile.Value.unitGameObject.index != unit.index)
                {
                    // If unit is an archer we don't need to calculate paths because archer can shoot over units, water etc.
                    if (!tile.Value.unitGameObject.unitGame.CanAttackAfterMove)
                    {
                        tile.Value.highlight.ChangeHighlight(HighlightTypes.highlight_attack);
                        GameManager.Instance.Highlight.highlightObjects.Add(tile.Value.highlight);
                    }
                    else
                    {
                        List<Node> path = _movement.CalculateShortestPath(unit.tile, tile.Value, true);

                        if (path != null && path.Count <= unit.unitGame.GetAttackMoveRange)
                        {
                            tile.Value.highlight.ChangeHighlight(HighlightTypes.highlight_attack);
                            GameManager.Instance.Highlight.highlightObjects.Add(tile.Value.highlight);
                        }
                    }                   
                }
            }
        }
        int count = GameManager.Instance.Highlight.highlightObjects.Count;
        GameManager.Instance.Highlight.isHighlightOn = count > 0;
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
            if (GameManager.Instance.Highlight.isHighlightOn && !GameManager.Instance.Highlight._movement.needsMoving && highlight.highlightTypeActive == HighlightTypes.highlight_attack)
            {
                UnitGameObject attackingUnit = GameManager.Instance.Highlight._unitSelected;
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
        if (TileHelper.IsTileWithinRange(attacker.transform.position, defender.transform.position, attacker.unitGame.attackRange))
        {
            attacker.unitGame.hasMoved = true;
            attacker.unitGame.hasAttacked = true;
            defender.unitGame.DecreaseHealth(3);
            attacker.unitGame.PlaySound(UnitSoundType.Attack);
            GameManager.Instance.Highlight.ClearNewHighlights();

            // Check if units are faces the wrong way
            FacingDirectionUnits(attacker, defender);

            // Start playing animation, loop in highlight class to stop animation after x amount of time.
            attacker.gameObject.GetComponent<Animator>().enabled = true;
            defender.gameObject.GetComponent<Animator>().enabled = true;
            GameManager.Instance.Highlight.AnimateFight = true;

            // Save animation info
            animInfo = new AnimationInfo();
            animInfo.attacker = attacker;
            animInfo.defender = defender;
            animInfo.defaultSpriteAttacker = attacker.gameObject.GetComponent<SpriteRenderer>().sprite;
            animInfo.defaultSpriteDefender = defender.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    void FacingDirectionUnits(UnitGameObject attacker, UnitGameObject defender)
    {
        
        Vector3 attDirection = defender.transform.position - attacker.transform.position;
        Vector3 defDirection = attacker.transform.position - defender.transform.position;

        Quaternion aq = new Quaternion(0, (attDirection.x >= 0 ? 0 : 180), 0, 0);
        Quaternion dq = new Quaternion(0, (defDirection.x >= 0 ? 0 : 180), 0, 0);
        attacker.transform.rotation = aq;
        defender.transform.rotation = dq;

        attacker.transform.FindChild("UnitHealth").rotation = aq;
        defender.transform.FindChild("UnitHealth").rotation = dq;

        attacker.transform.FindChild("UnitHealth").Rotate(new Vector3(0, 0, 0));
        defender.transform.FindChild("UnitHealth").Rotate(new Vector3(0, 0, 0));
    }
}