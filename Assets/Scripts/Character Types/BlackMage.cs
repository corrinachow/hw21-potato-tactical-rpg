using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackMage : Character
{
    public GameObject FireballPrefab;
    public GameObject IceballPrefab;
    public GameObject LightningPrefab;
    public GameObject MissilePrefab;

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
        projectile.Shoot(target, MissilePrefab);
    }
    
    public void Ice(GameObject target, int turn)
    {
       var projectile = weapon.GetComponent<Projectile>();
        projectile.Shoot(target, IceballPrefab);

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
        projectile.Shoot(target, LightningPrefab);

        // TODO: Figure out paralyze stat and skip turn for other target character
        Boolean isParalyzed = UnityEngine.Random.value <= 0.25;

          if (isParalyzed) {
            Buff paralyzeDebuff =  new Buff{
            Strength = 0,
            Magic = 0,
            Health = 0,
            Speed = -25,
        };

        target.SendMessage("ReceiveBuff", paralyzeDebuff);
        }
    }
    
    public void Fire(GameObject target, int turn)
    {
        var projectile = weapon.GetComponent<Projectile>();
        projectile.Shoot(target, FireballPrefab);
    }

    public override CharacterAction[] GetActions(int roundIndex)
    {
        var foes = GetCharacterTargets(TargetType.Foe, true);
        var targets = foes.Select(c => new GameTarget
        {
            GameObject = c.gameObject,
            TargetName = c.CharacterName,
            // TODO: Implement me (check collision, etc)
            IsAvailable = true,
            Team = Team == Team.Team1 ? Team.Team2 : Team.Team1,
        }).ToArray();

        return new []
        {
            new CharacterAction
            {
                ActionName = "Magic Missile",
                ActionIcon = GlobalResources.BlackMageMagicMissileSprite,
                Targets = targets,
                IsAvailable = true,
                OnInvoke = this.MagicMissile,
            },
            new CharacterAction
            {
                ActionName = "Fire",
                ActionIcon = GlobalResources.BlackMageFireAttackSprite,
                Targets = targets,
                IsAvailable = true,
                OnInvoke = this.Fire,
            },
            new CharacterAction
            {
                ActionName = "Lightening",
                ActionIcon = GlobalResources.BlackMageLightningAttackSprite,
                Targets = targets,
                IsAvailable = true,
                OnInvoke = this.Lightening,
            },
            new CharacterAction
            {
                ActionName = "Ice",
                ActionIcon = GlobalResources.BlackMageIceAttackSprite,
                Targets = targets,
                IsAvailable = true,
                OnInvoke = this.Ice,
            }
        };
    }
}
