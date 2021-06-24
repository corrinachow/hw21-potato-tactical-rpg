using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


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
    abstract public int TotalHealth { get; }
    abstract public int CurrentHealth { get; protected set; }
    abstract public int Magic { get; protected set; }
    abstract public int Strength { get; protected set; }
    abstract public int Speed { get; protected set; }
    abstract public int CritialHitPercent { get; protected set; }
    abstract public int Defense { get; protected set; }
    abstract public int MagicDefense { get; protected set; }

    abstract protected List<Spell> AppliedSpells { get; set; }

    abstract protected Vector2 Position { get; set; }

    public virtual void ReceiveDamage(Damage damage)
    {
        throw new NotImplementedException();
    }

    public virtual void ReceiveMagicDamage(Damage damage)
    {
        throw new NotImplementedException();
    }
    
    public virtual void ReceiveBuff(Buff buff){
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

    public virtual void SpecialAttack(){
        throw new NotImplementedException();
    }

    public virtual void Death(){
        Destroy(gameObject);
    }

    public virtual Team GetTeam()
    {
        return this.CompareTag("RedTeam") ? Team.Team2 : Team.Team1;
    }

    public virtual Sprite GetSprite()
    {
        var sprite = GetComponent<SpriteRenderer>().sprite;
        Assert.IsNotNull(sprite, "Could not find the character Sprite");

        return sprite;
    }

    public abstract CharacterAction[] GetActions();
}
