using System;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMage : Character
{
    public Team team;

    public GameObject weapon;

    public override Team Team => team;
    public override int TotalHealth { get; } = 75;
    public override string CharacterName => "White Mage";
    
    public GameObject[] Enemies {get; set;}
    public GameObject[] Teammates {get; set;}

    public Sprite playerSprite {get; set;}

    public int LastTurnSpecialAttack { get; set;} = 0;

    public override int CurrentHealth{ get; protected set;} = 75;
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
        base.CheckIfDead();
    }
    public override void ReceiveMagicDamage(Damage damage){
        CurrentHealth -= damage.damageAmount * ((100 - MagicDefense)/100);
        base.CheckIfDead();
    }

    public override void DealMagicDamage(){

    }

    public void MagicArrow(GameObject target, int turn)
    {
        var arrow = weapon.GetComponent<Projectile>();
        arrow.Shoot(target);
    }

    public void Heal(GameObject target, int turn)
    {
        Buff healBuff = new Buff
        {
            Strength = 0,
            Magic = 0,
            Health = (int)(25*UnityEngine.Random.Range(1,1.125f)* (2+(Magic*(Magic/256)))),
        };
    }

    public void Barrier(GameObject target, int turn)
    {
        LastTurnSpecialAttack = turn;

        Buff healBuff =  new Buff{
            Strength = 25,
            Magic = 0,
            Health = 0,
        };

        target.SendMessage("ReceiveBuff", healBuff);
    }

    public void Manaward(GameObject target, int turn){
        LastTurnSpecialAttack = turn;

        Buff healBuff =  new Buff{
            Strength = 0,
            Magic = 25,
            Health = 0,
        };

        target.SendMessage("ReceiveBuff", healBuff);
    }

    public virtual void Move(){

    }
    
    public override CharacterAction[] GetActions(int roundIndex)
    {
        return Array.Empty<CharacterAction>();
    }
}
