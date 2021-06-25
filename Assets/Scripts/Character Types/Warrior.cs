using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Warrior : Character
{
    public Team team;

    public override Team Team => team;
    public override string CharacterName => "Warrior";

    [SerializeField]
    private GameObject axe;

    public override int TotalHealth { get; } = 100;

    public override int CurrentHealth { get; protected set; } = 100;
    public override int Magic { get; protected set; } = 5;
    public override int Strength { get; protected set; } = 10;
    public override int Speed { get; protected set; } = 20;
    public override int CritialHitPercent { get; protected set; } = 3;
    public override int Defense { get; protected set; } = 10;
    public override int MagicDefense { get; protected set; } = 5;

    protected override List<Spell> AppliedSpells { get; set; }
    protected override Vector2 Position { get; set; }


    public override void ReceiveDamage(Damage damage)
    {
        CurrentHealth -= (int)(damage.damageAmount * ((100f - Defense) / 100));
        base.CheckIfDead();
    }
    public override void ReceiveMagicDamage(Damage damage)
    {
        CurrentHealth -= damage.damageAmount * ((100 - MagicDefense) / 100);
        base.CheckIfDead();
    }

    public override void DealMagicDamage()
    {

    }

    public virtual void Move()
    {

    }

    public void Sword(GameObject target, int turn)
    {
        var sword = transform.GetComponentInChildren<Sword>();
        sword.Swing();
    }

    public void Charge(GameObject target, int turn)
    {

    }

    public void Shield(GameObject target, int turn)
    {
        var shield = transform.GetComponentInChildren<Shield>();
        
        shield.OpenShield();
    }

    public void AxeThrow(GameObject target, int turn)
    {
        var axeProjectile = axe.GetComponent<Projectile>();
        axeProjectile.Shoot(target);
    }

    public override CharacterAction[] GetActions(int roundIndex)
    {
        var foes = GetCharacterTargets(TargetType.Foe, false);
        var targets = foes.Select(c => new GameTarget
        {
            GameObject = c.gameObject,
            TargetName = c.CharacterName,
            // TODO: Implement me (check collision, etc)
            IsAvailable = true,
            Team = Team == Team.Team1 ? Team.Team2 : Team.Team1,
        }).ToArray();

        return new[]
        {
            new CharacterAction
            {
                ActionName = "Sword attack",
                ActionIcon = GlobalResources.WarriorSwordAttackSprite,
                Targets = targets,
                IsAvailable = true,
                OnInvoke = this.Sword,
            },
            new CharacterAction
            {
                ActionName = "Charge",
                ActionIcon = GlobalResources.WarriorChargeSprite,
                Targets = targets,
                // TODO: Implement me (check effects, last turn used, etc)
                IsAvailable = true,
                OnInvoke = this.Charge,
            },
            new CharacterAction
            {
                ActionName = "Shield",
                ActionIcon = GlobalResources.WarriorDefendSprite,
                Targets = new[]
                {
                    new GameTarget
                    {
                        GameObject = gameObject,
                        IsAvailable = true,
                        TargetName = CharacterName,
                        Team = Team,
                    }
                },
                // TODO: Implement me (check effects, last turn used, etc)
                IsAvailable = true,
                OnInvoke = this.Shield,
            },
            new CharacterAction
            {
                ActionName = "Throw Axe",
                ActionIcon = GlobalResources.WarriorThrowSprite,
                Targets = targets,
                // TODO: Implement me (check effects, last turn used, etc)
                IsAvailable = true,
                OnInvoke = this.AxeThrow,
            }
        };
    }
}
