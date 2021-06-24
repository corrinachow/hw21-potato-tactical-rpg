using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMage : Character
{
    public Team team;

    public override Team Team => team;

    public override int TotalHealth { get; } = 75;
    public override string CharacterName => "Black Mage";
    
    public override int CurrentHealth{get; protected set;} = 75;
    public override int Magic { get; protected set; } = 15;
    public override int Strength { get; protected set; } = 5;
    public override int Speed { get; protected set; } = 15;
    public override int CritialHitPercent { get; protected set; } = 3;
    public override int Defense { get; protected set; } = 5; 
    public override int MagicDefense { get; protected set; } = 10;
    
    protected override List<Spell> AppliedSpells { get; set; }
    protected override Vector2 Position { get; set; }

    public override void ReceiveDamage(Damage damage){
        CurrentHealth -= damage.damageAmount * ((100 - Defense)/100);
        if (CurrentHealth <= 0){
            CurrentHealth = 0;
            Death();
        }   
    }
    public override void ReceiveMagicDamage(Damage damage){
        CurrentHealth -= damage.damageAmount * ((100 - MagicDefense)/100);
        if (CurrentHealth <= 0){
            CurrentHealth = 0;
            Death();
        }
    }

    public override void DealMagicDamage(){

    }

    public virtual void Move()
    {

    }

    public void MagicMissile(GameObject target, int turn) 
    {

    }
    
    public void Ice(GameObject target, int turn)
    {

    }
    
    public void Lightening(GameObject target, int turn)
    {

    }
    
    public void Fire(GameObject target, int turn)
    {

    }

    public override void Death()
    {
        // Destroy(gameObject);
    }

    public override CharacterAction[] GetActions(int roundIndex)
    {
        // TODO: To be implemented
        return Array.Empty<CharacterAction>();
    }
}
