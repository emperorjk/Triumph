using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Events;
using Assets.Scripts.Main;
using Assets.Scripts.Notification;
using Assets.Scripts.Tiles;
using Assets.Scripts.Units;
using UnityEngine;
using EventHandler = Assets.Scripts.Events.EventHandler;

namespace Assets.Scripts.UnitActions
{
    public class Attack : MonoBehaviour
    {
        private GameManager _manager;

        private void Awake()
        {
            EventHandler.register<OnHighlightClick>(BattlePreparation);
            EventHandler.register<OnAnimFight>(BattleSimulation);
        }

        private void Start()
        {
            _manager = GameObject.Find("_Scripts").GetComponent<GameManager>();
        }

        private void OnDestroy()
        {
            EventHandler.unregister<OnHighlightClick>(BattlePreparation);
            EventHandler.unregister<OnAnimFight>(BattleSimulation);
        }

        /// <summary>
        /// Show the attack highlights for the specified unit with the specified range.
        /// </summary>
        /// <param Name="unit"></param>
        /// <param Name="range"></param>
        /// <returns></returns>
        public int ShowAttackHighlights(UnitGameObject unit, int range)
        {
            foreach (var item in TileHelper.GetAllTilesWithinRange(unit.Tile.Coordinate, range))
            {
                foreach (var tile in item.Value)
                {
                    if (tile.Value.HasUnit() && tile.Value.unitGameObject.index != unit.index)
                    {
                        // If unit is an archer we don't need to calculate paths because archer can shoot over units, water etc.
                        if (!unit.UnitGame.CanAttackAfterMove && !tile.Value.IsFogShown)
                        {
                            tile.Value.Highlight.ChangeHighlight(HighlightTypes.highlight_attack);
                            _manager.Highlight.HighlightObjects.Add(tile.Value.Highlight);
                        }
                        else
                        {
                            List<Node> path = _manager.Movement.CalculateShortestPath(unit.Tile, tile.Value, true);

                            if (path != null && path.Count <= unit.UnitGame.GetAttackMoveRange && !tile.Value.IsFogShown)
                            {
                                tile.Value.Highlight.ChangeHighlight(HighlightTypes.highlight_attack);
                                _manager.Highlight.HighlightObjects.Add(tile.Value.Highlight);
                            }
                        }
                    }
                }
            }
            int count = _manager.Highlight.HighlightObjects.Count;
            _manager.Highlight.IsHighlightOn = count > 0;
            return count;
        }

        /// <summary>
        /// Get called whenever an OnHighlightClick is fired. If it is possible it will attack an enemy unit.
        /// </summary>
        /// <param Name="evt"></param>
        public void BattlePreparation(OnHighlightClick evt)
        {
            if (evt.highlight != null)
            {
                HighlightObject highlight = evt.highlight;
                if (_manager.Highlight.IsHighlightOn && !_manager.Movement.NeedsMoving &&
                    highlight.highlightTypeActive == HighlightTypes.highlight_attack)
                {
                    UnitGameObject attackingUnit = _manager.Highlight.UnitSelected;
                    UnitGameObject defendingUnit = highlight.Tile.unitGameObject;


                    if (!attackingUnit.UnitGame.HasAttacked)
                    {
                        if (!attackingUnit.UnitGame.HasMoved ||
                            (attackingUnit.UnitGame.HasMoved && attackingUnit.UnitGame.CanAttackAfterMove))
                        {
                            if (TileHelper.IsTileWithinRange(attackingUnit.transform.position,
                                defendingUnit.transform.position, attackingUnit.UnitGame.AttackRange))
                            {
                                attackingUnit.UnitGame.HasAttacked = true;
                                attackingUnit.UnitGame.HasMoved = true;
                                attackingUnit.UnitGame.PlaySound(UnitSoundType.Attack);

                                _manager.Highlight.ClearHighlights();

                                // Check if units are faces the wrong way
                                FacingDirectionUnits(attackingUnit, defendingUnit);

                                // Dispatch the animation fight event. But set the needsanimating to true.
                                OnAnimFight fight = new OnAnimFight();
                                fight.attacker = attackingUnit;
                                fight.defender = defendingUnit;
                                fight.needsAnimating = true;
                                EventHandler.dispatch(fight);
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
        /// <param Name="Attacker"></param>
        /// <param Name="Defender"></param>
        private void BattleSimulation(OnAnimFight evt)
        {
            if (evt.attacker != null && evt.defender != null && !evt.needsAnimating)
            {
                UnitGameObject attacker = evt.attacker;
                UnitGameObject defender = evt.defender;
                attacker.UnitGame.UpdateUnitColor();
                Unit attUnit = attacker.UnitGame;
                Unit defUnit = defender.UnitGame;

                float damageToDefender = attUnit.Damage*attUnit.GetStrength()*attUnit.GetGroundModifier()*
                                         attUnit.GetUnitModifier(defender.type);
                float damageToAttacker = defUnit.Damage*defUnit.GetStrength()*defUnit.GetGroundModifier()*
                                         defUnit.GetUnitModifier(attacker.type);
                damageToDefender = Mathf.Clamp(damageToDefender, 1f, float.MaxValue);
                damageToAttacker = Mathf.Clamp(damageToAttacker, 1f, float.MaxValue);

                defUnit.DecreaseHealth(damageToDefender);

                if (defUnit.AttackRange >= attUnit.AttackRange)
                {
                    attUnit.DecreaseHealth(damageToAttacker);
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
}