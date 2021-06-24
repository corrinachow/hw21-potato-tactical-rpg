using System;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMage : Character
{
    public Team team;

    public override Team Team => team;
    public override int TotalHealth { get; } = 75;
    
    public GameObject[] Enemies {get; set;}
    public GameObject[] Teammates {get; set;}

    public Sprite playerSprite {get; set;}

    public int LastTurnSpecialAttack {get; set;} = 0;

    public override int CurrentHealth{get; protected set;} = 75;
    public override int Magic { get; protected set; } = 15;
    public override int Strength { get; protected set; } = 5;
    public override int Speed { get; protected set; } = 15;
    public override int CritialHitPercent { get; protected set; } = 3;
    public override int Defense { get; protected set; } = 5; 
    public override int MagicDefense { get; protected set; } = 10;
    
    protected override List<Spell> AppliedSpells { get; set; }
    protected override Vector2 Position { get; set; }

    public void Start(){
        if(team == Team.Team1){
            Enemies = GameObject.FindGameObjectsWithTag("RedTeam");
            Teammates = GameObject.FindGameObjectsWithTag("BlueTeam");
        }
        else{
            Enemies = GameObject.FindGameObjectsWithTag("BlueTeam");
            Teammates = GameObject.FindGameObjectsWithTag("RedTeam");
        }
    }

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

    public void MagicArrow(GameObject target, int turn){

    }

    public void Heal(GameObject target, int turn){
        LastTurnSpecialAttack = turn;

        Buff healBuff =  new Buff{
            isShieldOpen = false,
            hasManaward = false,
            hasBarrier= false,
            health = (int)(25*UnityEngine.Random.Range(1,1.125f)* (2+(Magic*(Magic/256)))),
        };

        target.SendMessage("ReceiveBuff", healBuff);

    }

    public void Barrier(GameObject target, int turn){
        LastTurnSpecialAttack = turn;

        Buff healBuff =  new Buff{
            isShieldOpen = false,
            hasManaward = false,
            hasBarrier= true,
            health = 0,
        };

        target.SendMessage("ReceiveBuff", healBuff);
    }

    public void Manaward(GameObject target, int turn){
        LastTurnSpecialAttack = turn;

        Buff healBuff =  new Buff{
            isShieldOpen = false,
            hasManaward = true,
            hasBarrier= false,
            health = 0,
        };

        target.SendMessage("ReceiveBuff", healBuff);
    }

    public virtual void Move(){

    }

    public override void Death(){
        Destroy(gameObject);
    }
    
    public override CharacterAction[] GetActions()
    {
        return Array.Empty<CharacterAction>();
    }
}
