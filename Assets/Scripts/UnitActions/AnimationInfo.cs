using Assets.Scripts.Events;
using Assets.Scripts.Units;
using UnityEngine;
using EventHandler = Assets.Scripts.Events.EventHandler;

namespace Assets.Scripts.UnitActions
{
    public class AnimationInfo : MonoBehaviour
    {
        public UnitGameObject Defender { get; set; }
        public UnitGameObject Attacker { get; set; }
        public Sprite DefaultSpriteDefender { get; set; }
        public Sprite DefaultSpriteAttacker { get; set; }
        public float FightTime { get; set; }

        public bool IsAnimateFight { get; set; }

        private void Awake()
        {
            FightTime = 1f;
            EventHandler.register<OnAnimFight>(OnFightAnim);
        }

        private void OnDestroy()
        {
            EventHandler.unregister<OnAnimFight>(OnFightAnim);
        }

        private void OnFightAnim(OnAnimFight evt)
        {
            if (evt.attacker != null && evt.defender != null && evt.needsAnimating)
            {
                Defender = evt.defender;
                Attacker = evt.attacker;
                DefaultSpriteAttacker = Attacker.gameObject.GetComponent<SpriteRenderer>().sprite;
                DefaultSpriteDefender = Defender.gameObject.GetComponent<SpriteRenderer>().sprite;

                Attacker.gameObject.GetComponent<Animator>().enabled = true;

                if (Defender.UnitGame.AttackRange >= Attacker.UnitGame.AttackRange)
                {
                    Defender.gameObject.GetComponent<Animator>().enabled = true;
                }

                IsAnimateFight = true;
            }
        }


        private void Update()
        {
            if (IsAnimateFight)
            {
                FightTime -= Time.deltaTime;

                if (FightTime <= 0)
                {
                    IsAnimateFight = false;

                    Attacker.gameObject.GetComponent<Animator>().enabled = false;
                    Defender.gameObject.GetComponent<Animator>().enabled = false;

                    Attacker.gameObject.GetComponent<SpriteRenderer>().sprite = DefaultSpriteAttacker;
                    Defender.gameObject.GetComponent<SpriteRenderer>().sprite = DefaultSpriteDefender;

                    Attacker.UpdateHealthText();
                    Defender.UpdateHealthText();
                    FightTime = 1f;

                    // Create the animation fight event. But set the needsanimating to false. Meaning that the method Attack.BattleSimulation() is called.
                    OnAnimFight fight = new OnAnimFight();
                    fight.attacker = Attacker;
                    fight.defender = Defender;
                    fight.needsAnimating = false;
                    
                    EventHandler.dispatch(fight);
                }
            }
        }
    }
}