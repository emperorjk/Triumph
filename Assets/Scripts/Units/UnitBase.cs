using UnityEngine;
using System.Collections;

public abstract class UnitBase {

    protected UnitBase(UnitGameObject game, int health, float damage, int attackRange, int moveRange, int cost, bool isHero)
    {
        this.unitGameObject = game;
        this.hasMoved = false;
        this.hasAttacked = false;
        this.isHero = false;
        this.currentHealth = health;
        this.health = health;
        this.damage = damage;
        this.attackRange = attackRange;
        this.moveRange = moveRange;
        this.cost = cost;
        this.isHero = isHero;
    }

    public abstract int GetAttackMoveRange { get; }
    public abstract bool CanAttackAfterMove { get; }
    public abstract int FowLineOfSightRange { get; }
    public abstract void PlaySound(UnitSoundType soundType);
    
    public void DecreaseHealth(int damage) 
    {
        this.currentHealth -= damage;
        this.unitGameObject.UpdateCapturePointsText();

        if (this.currentHealth <= 0) 
        {
            GameManager.Instance.DestroyUnitGameObjects(unitGameObject);
        }
    }

    public void IncreaseHealth(int recovery)
    {
        this.currentHealth += recovery;
        // If we later have some sort of healing make sure that it cannot go over its initial full health.
        this.currentHealth = Mathf.Clamp(this.currentHealth, 0, this.health);
        //if (this.currentHealth >= this.health) { this.currentHealth = this.health; }
        this.unitGameObject.UpdateCapturePointsText();
    }

    private void UpdateUnitColor()
    {
        unitGameObject.gameObject.renderer.material.color = hasMoved && hasAttacked ? Color.gray : Color.white;
    }

    public bool hasMoved {
        get { return moved; } 
        set 
        {
            moved = value;
            UpdateUnitColor(); 
        } 
    }
    public bool hasAttacked
    {
        get { return attacked; }
        set
        {
            attacked = value;
            UpdateUnitColor();
        }
    }
    private bool moved = false;
    private bool attacked = false;

    public bool isHero { get; private set; }
    public int health { get; private set; }
    public int currentHealth { get; private set; }
    public float damage { get; private set; }
    public int attackRange { get; private set; }
    public int moveRange { get; private set; }
    public int cost { get; private set; }
    public UnitGameObject unitGameObject { get; private set; }
}
