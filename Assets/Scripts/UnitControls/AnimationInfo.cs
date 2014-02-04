using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AnimationInfo : MonoBehaviour
{
    public UnitGameObject defender { get; set; }
    public UnitGameObject attacker { get; set; }
    public Sprite defaultSpriteDefender { get; set; }
    public Sprite defaultSpriteAttacker { get; set; }
    public float FightTime { get; set; }

    public bool IsAnimateFight { get; set; }

    void Awake()
    {
        FightTime = 1f;
        EventHandler.register<OnAnimFight>(OnFightAnim);
    }

    void OnFightAnim(OnAnimFight evt)
    {
        if(evt.attacker != null && evt.defender != null && evt.needsAnimating)
        {
            defender = evt.defender;
            attacker = evt.attacker;
            defaultSpriteAttacker = attacker.gameObject.GetComponent<SpriteRenderer>().sprite;
            defaultSpriteDefender = defender.gameObject.GetComponent<SpriteRenderer>().sprite;

            attacker.gameObject.GetComponent<Animator>().enabled = true;

            if (defender.UnitGame.AttackRange >= attacker.UnitGame.AttackRange)
                defender.gameObject.GetComponent<Animator>().enabled = true;

            IsAnimateFight = true;
        }
    }

    
    void Update()
    {
        if (IsAnimateFight)
        {
            FightTime -= Time.deltaTime;

            if (FightTime <= 0)
            {
                IsAnimateFight = false;

                attacker.gameObject.GetComponent<Animator>().enabled = false;
                defender.gameObject.GetComponent<Animator>().enabled = false;
                
                attacker.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSpriteAttacker;
                defender.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSpriteDefender;
                                
                attacker.UnitHealthText.renderer.enabled = true;
                defender.UnitHealthText.renderer.enabled = true;
                FightTime = 1f;

                // Create the animation fight event. But set the needsanimating to false. Meaning that the method Attack.BattleSimulation() is called.
                OnAnimFight fight = new OnAnimFight();
                fight.attacker = attacker;
                fight.defender = defender;
                fight.needsAnimating = false;

                EventHandler.dispatch<OnAnimFight>(fight);
            }
        }
    }

    void OnDestroy()
    {
        EventHandler.unregister<OnAnimFight>(OnFightAnim);
    }
}

