using UnityEngine;
using System.Collections;

public abstract class UnitBase {

    protected UnitBase(UnitGameObject game, int health, float damage, int attackRange, int baseAttackRange, int moveRange, int cost)
    {
        this.unitGameObject = game;
        this.hasMoved = false;
        this.hasAttacked = false;
        this.isHero = false;
        this.health = health;
        this.damage = damage;
        this.attackRange = attackRange;
        this.baseAttackRange = baseAttackRange;
        this.moveRange = moveRange;
        this.cost = cost;
    }
    public bool hasMoved { get; set; }
    public bool hasAttacked { get; set; }
    public bool isHero { get; set; }
    public int health { get; set; }
    public float damage { get; set; }
    public int attackRange { get; set; }
    public int baseAttackRange { get; set; }
    public int moveRange { get; set; }
    public int cost { get; private set; }
    public UnitGameObject unitGameObject { get; private set; }
}
