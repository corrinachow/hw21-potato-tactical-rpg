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

    public override void ReceiveDamage(Damage damage)
    {
        CurrentHealth -= damage.damageAmount * ((100 - Defense)/100);
        base.CheckIfDead();
    }
    public override void ReceiveMagicDamage(Damage damage)
    {
        CurrentHealth -= damage.damageAmount * ((100 - MagicDefense)/100);
        base.CheckIfDead();
    }

    public override void DealMagicDamage()
    {

    }

    public virtual void Move()
    {

    }

    public void MagicMissile(GameObject target, int turn) 
    {
       var projectile = weapon.GetComponent<Projectile>();
        projectile.Shoot(target);
    }
    
    public void Ice(GameObject target, int turn)
    {
       var projectile = weapon.GetComponent<Projectile>();
        projectile.Shoot(target);

        Boolean isSlowed = UnityEngine.Random.value <= 0.25;

        if (isSlowed) {
            Buff slowDebuff =  new Buff{
            Strength = 0,
            Magic = 0,
            Health = 0,
            Speed = -5,
        };

        target.SendMessage("ReceiveBuff", slowDebuff);
        }
    }
    
    public void Lightening(GameObject target, int turn)
    {
       var projectile = weapon.GetComponent<Projectile>();
        projectile.Shoot(target);

        Boolean isParalyzed = UnityEngine.Random.value <= 0.25;

          if (isParalyzed) {
            Buff slowDebuff =  new Buff{
            Strength = 0,
            Magic = 0,
            Health = 0,
            Speed = -25,
        };

        target.SendMessage("ReceiveBuff", slowDebuff);
        }
    }
    
    public void Fire(GameObject target, int turn)
    {
        var projectile = weapon.GetComponent<Projectile>();
        projectile.Shoot(target);

    }

    public override CharacterAction[] GetActions(int roundIndex)
    {
        var targets = GetAvailableTargets();

        if (targets.Length == 0)
            return Array.Empty<CharacterAction>();

        return new CharacterAction[]
        {
            new CharacterAction
            {
                ActionName = "Magic Missile",
                ActionIcon = null,
                Targets = Array.Empty<GameTarget>(),
                OnInvoke = this.MagicMissile,
            },
            new CharacterAction
            {
                ActionName = "Fire",
                ActionIcon = null,
                Targets = Array.Empty<GameTarget>(),
                OnInvoke = this.Fire,
            },
            new CharacterAction
            {
                ActionName = "Lightening",
                ActionIcon = null,
                Targets = Array.Empty<GameTarget>(),
                OnInvoke = this.Lightening,
            },
            new CharacterAction
            {
                ActionName = "Ice",
                ActionIcon = null,
                Targets = Array.Empty<GameTarget>(),
                OnInvoke = this.Ice,
            }
        };
    }
}
