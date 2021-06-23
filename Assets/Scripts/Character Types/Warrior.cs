using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Character
{
    protected override int TotalHealth { get; } = 100;
    protected override int CurrentHealth {get; set;} = 100;
    protected override int Magic { get; set; } = 5;
    protected override int Strength { get; set; } = 10;
    protected override int Speed { get; set; } = 10;
    protected override int CritialHitPercent { get; set; } = 3;
    protected override int Defense { get; set; } = 10; 
    protected override int MagicDefense { get; set; } = 5;
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

}
