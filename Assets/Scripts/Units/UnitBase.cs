using UnityEngine;
using System.Collections;

public abstract class UnitBase {

    protected UnitBase(UnitGameObject game, int health, float damage, int attackRange, int moveRange, int cost, bool isHero)
    {
        this.UnitGameObject = game;
        this.hasMoved = false;
        this.hasAttacked = false;
        this.isHero = false;
        this.CurrentHealth = health;
        this.MaxHealth = health;
        this.Damage = damage;
        this.AttackRange = attackRange;
        this.MoveRange = moveRange;
        this.Cost = cost;
        this.isHero = isHero;
    }

    public abstract int GetAttackMoveRange { get; }
    public abstract bool CanAttackAfterMove { get; }
    public abstract int FowLineOfSightRange { get; }
    public abstract void PlaySound(UnitSoundType soundType);
    
    public void DecreaseHealth(int damage) 
    {
        this.CurrentHealth -= damage;
        this.UnitGameObject.UpdateCapturePointsText();

        if (this.CurrentHealth <= 0) 
        {
            this.UnitGameObject.DestroyUnit();
        }
    }

    public void IncreaseHealth(int recovery)
    {
        this.CurrentHealth += recovery;
        // If we later have some sort of healing make sure that it cannot go over its initial full health.
        this.CurrentHealth = Mathf.Clamp(this.CurrentHealth, 0, this.MaxHealth);
        //if (this.currentHealth >= this.health) { this.currentHealth = this.health; }
        this.UnitGameObject.UpdateCapturePointsText();
    }

    public void UpdateUnitColor()
    {
        UnitGameObject.gameObject.renderer.material.color = hasMoved && hasAttacked ? Color.gray : Color.white;
    }

    public bool hasMoved {
        get { return _moved; } 
        set 
        {
            _moved = value;
            //UpdateUnitColor(); 
        } 
    }
    public bool hasAttacked
    {
        get { return _attacked; }
        set
        {
            _attacked = value;
            //UpdateUnitColor();
        }
    }
    private bool _moved = false;
    private bool _attacked = false;

    public bool isHero { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public float Damage { get; private set; }
    public int AttackRange { get; private set; }
    public int MoveRange { get; private set; }
    public int Cost { get; private set; }
    public UnitGameObject UnitGameObject { get; private set; }
}
