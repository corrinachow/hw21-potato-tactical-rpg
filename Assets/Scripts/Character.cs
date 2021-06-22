using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Spell
{
    public string Name;
    public string AppliedByPlayer;
    public int AppliedTurn;
    public int ReduceAttackPercent;
    public int ReduceMagicAttackPercent;
}


// Abstract class that all other characters will
// inherit from, such as White Mage / Black Mage / Warrior
public abstract class Character : MonoBehaviour
{
    abstract protected int TotalHealth { get; set; }
    abstract protected int CurrentHealth { get; set; }

    abstract protected int Magic { get; set; }
    abstract protected int Stamina { get; set; }
    abstract protected int Strength { get; set; }
    abstract protected int Agility { get; set; }
    abstract protected int Speed { get; set; }
    abstract protected int Range { get; set; }

    abstract protected int CritialHitPercent { get; set; }
    abstract protected int MagicCritialHitPercent { get; set; }

    abstract protected int Attack { get; set; }
    abstract protected int MagicAttack { get; set; }

    abstract protected int Defense { get; set; }
    abstract protected int MagicDefense { get; set; }

    abstract protected List<Spell> AppliedSpells { get; set; }

    abstract protected Vector2 Position { get; set; }

    public virtual void ReceiveDamage()
    {
        throw new NotImplementedException();
    }

    public virtual void ReceiveMagicDamage()
    {
        throw new NotImplementedException();
    }

    public virtual void DealDamage()
    {
        throw new NotImplementedException();
    }

    public virtual void DealMagicDamage()
    {
        throw new NotImplementedException();
    }

    public virtual void Move()
    {
        throw new NotImplementedException();
    }
}
