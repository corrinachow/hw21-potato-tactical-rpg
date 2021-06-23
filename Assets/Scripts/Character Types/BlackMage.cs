using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMage : Character
{
    protected override int TotalHealth { get; } = 75;
    protected override int CurrentHealth{get; set;} = 75;
    protected override int Magic { get; set; } = 15;
    protected override int Strength { get; set; } = 5;
    protected override int Speed { get; set; } = 15;
    protected override int CritialHitPercent { get; set; } = 3;
    protected override int Defense { get; set; } = 5; 
    protected override int MagicDefense { get; set; } = 10;
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

    public override void SpecialAttack(){

    }

    public override void DealMagicDamage(){

    }

    public virtual void Move(){

    }

    public override void Death(){
        // Destroy(gameObject);
    }
}
