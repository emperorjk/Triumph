using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attack : MonoBehaviour
{
    void Awake ()
    {
        EventHandler.register<OnHighlightClick>(BattlePreparation);
        EventHandler.register<OnAnimFight>(BattleSimulation);
    }

    void OnDestroy()
    {
        EventHandler.unregister<OnHighlightClick>(BattlePreparation);
        EventHandler.unregister<OnAnimFight>(BattleSimulation);
    }

    /// <summary>
    /// Show the attack highlights for the specified unit with the specified range.
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public int ShowAttackHighlights(UnitGameObject unit, int range)
    {
        foreach (KeyValuePair<int, Dictionary<int, Tile>> item in TileHelper.GetAllTilesWithinRange(unit.Tile.Coordinate, range))
        {
            foreach (KeyValuePair<int, Tile> tile in item.Value)
            {
                if (tile.Value.HasUnit() && tile.Value.unitGameObject.index != unit.index)
                {
                    // If unit is an archer we don't need to calculate paths because archer can shoot over units, water etc.
                    if (!unit.UnitGame.CanAttackAfterMove)
                    {
                        tile.Value.highlight.ChangeHighlight(HighlightTypes.highlight_attack);
                        GameManager.Instance.Highlight.HighlightObjects.Add(tile.Value.highlight);
                    }
                    else
                    {
                        List<Node> path = GameManager.Instance.Movement.CalculateShortestPath(unit.Tile, tile.Value, true);

                        if (path != null && path.Count <= unit.UnitGame.GetAttackMoveRange)
                        {
                            tile.Value.highlight.ChangeHighlight(HighlightTypes.highlight_attack);
                            GameManager.Instance.Highlight.HighlightObjects.Add(tile.Value.highlight);  
                        }
                    }                   
                }
            }
        }
        int count = GameManager.Instance.Highlight.HighlightObjects.Count;
        GameManager.Instance.Highlight.IsHighlightOn = count > 0;
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
            if (GameManager.Instance.Highlight.IsHighlightOn && !GameManager.Instance.Movement.NeedsMoving && highlight.highlightTypeActive == HighlightTypes.highlight_attack)
            {
                UnitGameObject attackingUnit = GameManager.Instance.Highlight.UnitSelected;
                UnitGameObject defendingUnit = highlight.tile.unitGameObject;


                if (!attackingUnit.UnitGame.hasAttacked)
                {
                    if (!attackingUnit.UnitGame.hasMoved || (attackingUnit.UnitGame.hasMoved && attackingUnit.UnitGame.CanAttackAfterMove))
                    {
                        if (TileHelper.IsTileWithinRange(attackingUnit.transform.position, defendingUnit.transform.position, attackingUnit.UnitGame.AttackRange))
                        {
                            attackingUnit.UnitGame.hasAttacked = true;
                            attackingUnit.UnitGame.hasMoved = true;
                            attackingUnit.UnitGame.PlaySound(UnitSoundType.Attack);

                            GameManager.Instance.Highlight.ClearHighlights();

                            // Check if units are faces the wrong way
                            FacingDirectionUnits(attackingUnit, defendingUnit);

                            // Dispatch the animation fight event. But set the needsanimating to true.
                            OnAnimFight fight = new OnAnimFight();
                            fight.attacker = attackingUnit;
                            fight.defender = defendingUnit;
                            fight.needsAnimating = true;
                            EventHandler.dispatch<OnAnimFight>(fight);
                        }
                        else
                        {
                            Notificator.Notify("Move to this unit to attack!", 1f);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// The method which does all the complicated battle simulation. From calculating damage to dealing damage and so forth.
    /// Gets called when the event OnAnimFight is fired. When the needsAnimating property is false it will execute this code.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    private void BattleSimulation(OnAnimFight evt)
    {
        if(evt.attacker != null && evt.defender != null && !evt.needsAnimating)
        {
            UnitGameObject attacker = evt.attacker;
            UnitGameObject defender = evt.defender;
            attacker.UnitGame.UpdateUnitColor();

            float attackerDamage = (attacker.UnitGame.Damage * attacker.UnitGame.GetModifier() * attacker.UnitGame.GetBaseModifier(defender.type) * 3 * attacker.UnitGame.GetStrength());
            float defenderDamage = (defender.UnitGame.Damage * defender.UnitGame.GetModifier() * defender.UnitGame.GetBaseModifier(attacker.type) * 3 * defender.UnitGame.GetStrength());

            defender.UnitGame.DecreaseHealth((int)Math.Ceiling(attackerDamage));
            
            if (defender.UnitGame.AttackRange >= attacker.UnitGame.AttackRange)
            {
                attacker.UnitGame.DecreaseHealth((int)Math.Ceiling(defenderDamage));
            }

            CheckUnitsHealth(attacker, defender);        
        }
    }

    private void CheckUnitsHealth(UnitGameObject attacker, UnitGameObject defender)
    {
        if (!attacker.UnitGame.IsAlive() && !defender.UnitGame.IsAlive())
        {
            defender.UnitGame.OnDeath();
            attacker.UnitGame.OnDeath();
        }
        else if (!attacker.UnitGame.IsAlive())
        {
            attacker.UnitGame.OnDeath();
            defender.UnitGame.AddLoot(10);
        }
        else if (!defender.UnitGame.IsAlive())
        {
            defender.UnitGame.OnDeath();
            attacker.UnitGame.AddLoot(10);
        }
    }

    private void FacingDirectionUnits(UnitGameObject attacker, UnitGameObject defender)
    {
        Vector3 attDirection = defender.transform.position - attacker.transform.position;
        Vector3 defDirection = attacker.transform.position - defender.transform.position;

        Quaternion attackerQ = new Quaternion(0, (attDirection.x >= 0 ? 0 : 180), 0, 0);
        Quaternion defenderQ = new Quaternion(0, (defDirection.x >= 0 ? 0 : 180), 0, 0);
        attacker.transform.rotation = attackerQ;
        defender.transform.rotation = defenderQ;

        Quaternion attackerHealthQ = new Quaternion(0, 0, 0, (attacker.transform.position.y > 0 ? 0 : 180));
        Quaternion defenderHealthQ = new Quaternion(0, 0, 0, (defender.transform.position.y > 0 ? 0 : 180));
        attacker.UnitHealthText.transform.rotation = attackerHealthQ;
        defender.UnitHealthText.transform.rotation = defenderHealthQ;

        // While attacking don't show the UnitHealth
        attacker.UnitHealthText.renderer.enabled = false;
        defender.UnitHealthText.renderer.enabled = false;
    }
}