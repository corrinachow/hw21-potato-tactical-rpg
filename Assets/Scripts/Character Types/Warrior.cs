using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Character
{
    public override int TotalHealth { get; } = 100;
    
    public override int CurrentHealth {get; protected set;} = 100;
    public override int Magic { get; protected set; } = 5;
    public override int Strength { get; protected set; } = 10;
    public override int Speed { get; protected set; } = 10;
    public override int CritialHitPercent { get; protected set; } = 3;
    public override int Defense { get; protected set; } = 10; 
    public override int MagicDefense { get; protected set; } = 5;
    
    protected bool isShieldOpen{get; set;} = false;
    protected override List<Spell> AppliedSpells { get; set; }
    protected override Vector2 Position { get; set; }


    public override void ReceiveDamage(Damage damage){
        CurrentHealth -= (int)(damage.damageAmount * ((100f - Defense)/100));
        if (CurrentHealth <= 0){
            CurrentHealth = 0;
            Death();
        }

        Debug.Log(" Damage CurrentHealth: "+ CurrentHealth);
    }
    public override void ReceiveMagicDamage(Damage damage){
        CurrentHealth -= damage.damageAmount * ((100 - MagicDefense)/100);
        if (CurrentHealth <= 0){
            CurrentHealth = 0;
            Death();
        }

        Debug.Log("Magic CurrentHealth: "+ CurrentHealth);
    }

    public override void ReceiveBuff(Buff buff){
        
    }

    public override void SpecialAttack(){

    }

    public override void DealMagicDamage(){

    }

    public virtual void Move(){

    }

    public void Charge(){

    }

    public override void Death(){
        // Destroy(gameObject);
    }

    public override CharacterAction[] GetActions()
    {
        // TODO: To be implemented
        return Array.Empty<CharacterAction>();
    }
}
