using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMage : Character
{
    public Team team;

    public override Team Team => team;

    public GameObject weapon;

    public override int TotalHealth { get; } = 75;
    
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

    public virtual void Move(){

    }

    public void MagicMissile(string target){

    }
    
    public void Ice(string target){

    }
    
    public void Lightening(string target){

    }
    
    public void Fire(GameObject target)
    {
        var wand = weapon.GetComponent<MagicWand>();
        if (wand.IsInSight(target))
        {
            wand.Shoot(target);
        }
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
