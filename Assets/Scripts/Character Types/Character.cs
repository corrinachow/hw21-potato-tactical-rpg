using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private static readonly Dictionary<Team, string> FriendlyTagMap = new Dictionary<Team, string>
    {
        {Team.Team1, "BlueTeam"},
        {Team.Team2, "RedTeam"}
    };

    private static readonly Dictionary<Team, string> FoesTagMap = new Dictionary<Team, string>
    {
        {Team.Team1, "RedTeam"},
        {Team.Team2, "BlueTeam"},
    };

    abstract public int TotalHealth { get; }
    abstract public Team Team { get; }
    abstract public string CharacterName { get; }
    
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

    public virtual void Death(){
        Destroy(gameObject);
    }

    public virtual Sprite GetSprite()
    {
        var sprite = GetComponent<SpriteRenderer>().sprite;
        Assert.IsNotNull(sprite, "Could not find the character Sprite");

        return sprite;
    }

    public abstract CharacterAction[] GetActions(int roundIndex);

    public virtual Character[] GetCharacterTargets(TargetType type, bool includeSelf)
    {
        var tagMap = type == TargetType.Friend ? FriendlyTagMap : FoesTagMap;
        var tag = tagMap[Team];

        var targets = GameObject.FindGameObjectsWithTag(tag)
            .Select(go => go.GetComponent<Character>())
            .Where(c => c != null)
            .Where(c => !includeSelf && c.name != name)
            .ToArray();

        return targets;
    }
}
