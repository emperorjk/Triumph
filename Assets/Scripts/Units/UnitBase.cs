using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBase {

    public UnitBase(UnitGameObject game, bool isHero, int attackRange, int moveRange, bool canAttackAfterMove, int maxHealth,
        float damage, int cost, int fowLos, int baseLoot, Dictionary<UnitTypes, float> modifiers)
    {
        this.UnitGameObject = game;
        this.isHero = isHero;
        this.hasMoved = false;
        this.hasAttacked = false;
        this.AttackRange = attackRange;
        this.MoveRange = moveRange;
        this.CanAttackAfterMove = canAttackAfterMove;
        this.CurrentHealth = maxHealth;
        this.MaxHealth = maxHealth;
        this.Damage = damage;
        this.Cost = cost;
        this.FowLineOfSightRange = fowLos;
        this.BaseLoot = baseLoot;
        this.CurrentLoot = baseLoot;
        this.modifiers = modifiers;
    }

    public UnitGameObject UnitGameObject { get; private set; }
    public bool isHero { get; private set; }
    public bool hasMoved
    {
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

    public int AttackRange
    {
        get
        {
            return GameManager.Instance.Fow.isFowActive ? 1 : _AttackRange;
        }
        private set { _AttackRange = value; }
    }

    private int _AttackRange;
    public int MoveRange
    {
        get
        {
            return GameManager.Instance.Fow.isFowActive ? Mathf.Clamp(_MoveRange - 1, 1, int.MaxValue) : _MoveRange;
        }
        private set { _MoveRange = value; }
    }

    private int _MoveRange;

    public int GetAttackMoveRange
    {
        get
        {
            return CanAttackAfterMove ? AttackRange + MoveRange : AttackRange;
        }
    }
    public bool CanAttackAfterMove { get; set; }
    public int CurrentLoot { get; private set; }

    public int DeliverLoot()
    {
        int amount = 0;

        if(CurrentLoot > BaseLoot)
        {
            amount = CurrentLoot - BaseLoot;
            CurrentLoot = BaseLoot;
        }
        return amount;
    }

    public void AddLoot(int loot) 
    { 
        CurrentLoot += loot; 
    }

    public int BaseLoot { get; private set; }

    public int FowLineOfSightRange { get; set; }
    public void PlaySound(UnitSoundType soundType)
    {
        GameManager.Instance.UnitSounds.PlaySound(UnitGameObject.type, soundType);
    }
    
    public void DecreaseHealth(int damage) 
    {
        this.CurrentHealth -= damage;
        this.UnitGameObject.UpdateCapturePointsText();
    }

    public void IncreaseHealth(int recovery)
    {
        this.CurrentHealth += recovery;
        // If we later have some sort of healing make sure that it cannot go over its initial full health.
        this.CurrentHealth = Mathf.Clamp(this.CurrentHealth, 0, this.MaxHealth);
        //if (this.currentHealth >= this.health) { this.currentHealth = this.health; }
        this.UnitGameObject.UpdateCapturePointsText();
    }

    public bool CheckAlive()
    {
        if (this.CurrentHealth <= 0)
        {
            return false;
        }

        return true;
    }

    public void OnDeath()
    {
        GameObject loot = ((GameObject)GameObject.Instantiate(Resources.Load<GameObject>(FileLocations.lootPrefab)));
        this.UnitGameObject.Tile.Loot = loot.GetComponent<Loot>();
        this.UnitGameObject.Tile.Loot.SetLoot(this.CurrentLoot);
        loot.transform.position = this.UnitGameObject.Tile.gameObject.transform.position;

        this.UnitGameObject.DestroyUnit();
    }

    public void UpdateUnitColor()
    {
        UnitGameObject.gameObject.renderer.material.color = hasMoved && hasAttacked ? Color.gray : Color.white;
    }

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public float Damage { get; private set; }
    public float GetStrength() { return CurrentHealth / 10f; }
    public int Cost { get; private set; }

    private Dictionary<UnitTypes, float> modifiers { get; set; }
    public float GetBaseModifier(UnitTypes type) { return modifiers[type]; }

    public float GetModifier()
    {
        if(UnitGameObject.Tile.HasBuilding())
        {
            if(UnitGameObject.Tile.buildingGameObject.index != UnitGameObject.index)
            {
                return 1f;
            }
            return UnitGameObject.Tile.buildingGameObject.buildingGame.GetModifier(UnitGameObject.type);
        }
        else
        {
            return UnitGameObject.Tile.environmentGameObject.environmentGame.GetModifier(UnitGameObject.type);
        }
    }
}
