using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AnimationInfo
{
    public UnitGameObject defender { get; set; }
    public UnitGameObject attacker { get; set; }
    public Sprite defaultSpriteDefender { get; set; }
    public Sprite defaultSpriteAttacker { get; set; }
    public float FightTime { get; set; }

    public AnimationInfo()
    {
        FightTime = 1f;
    }

    public void OnUpdate()
    {
        if (GameManager.Instance.AnimateFight)
        {
            FightTime -= Time.deltaTime;

            if (FightTime <= 0)
            {
                GameManager.Instance.AnimInfo.defender.gameObject.GetComponent<Animator>().enabled = false;
                GameManager.Instance.AnimInfo.attacker.gameObject.GetComponent<Animator>().enabled = false;
                GameManager.Instance.AnimInfo.defender.gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.AnimInfo.defaultSpriteDefender;
                GameManager.Instance.AnimInfo.attacker.gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.AnimInfo.defaultSpriteAttacker;

                GameManager.Instance.AnimateFight = false;

                // Decrease damage after animation. We need to change this later.
                defender.unitGame.DecreaseHealth((int)attacker.unitGame.damage * 3);
                attacker.unitGame.DecreaseHealth((int)defender.unitGame.damage * 3);

                attacker.transform.FindChild("UnitHealth").renderer.enabled = true;
                defender.transform.FindChild("UnitHealth").renderer.enabled = true;
                FightTime = 1f;
            }
        }
    }
}

