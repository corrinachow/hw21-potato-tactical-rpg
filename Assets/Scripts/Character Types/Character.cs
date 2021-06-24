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
    abstract public Team Team { get; }
    
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
        Magic += buff.Magic;
        Strength += buff.Strength;
        CurrentHealth += buff.Health;
    }

    public virtual void DealDamage()
    {
        throw new NotImplementedException();
    }

    public virtual void DealMagicDamage()
    {
        throw new NotImplementedException();
    }

    public virtual void CheckIfDead()
    {
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Death();
        }
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual Sprite GetSprite()
    {
        var sprite = GetComponent<SpriteRenderer>().sprite;
        Assert.IsNotNull(sprite, "Could not find the character Sprite");

        return sprite;
    }

    public GameObject[] GetAvailableTargets()
    {
        (string enemyTag, string enemyLayer) enemyTuple = gameObject.transform.CompareTag("RedTeam") ? ("BlueTeam", "BlueTarget") : ("RedTeam", "RedTarget");
        var allEnemyTargets = GameObject.FindGameObjectsWithTag(enemyTuple.enemyTag);

        // check if each target is in sight
        var filtered = new List<GameObject>();
        foreach (var target in allEnemyTargets)
        {
            if (IsInSight(target, enemyTuple.enemyLayer, enemyTuple.enemyTag, gameObject.transform.position))
            {
                filtered.Add(target);
            }
        }

        return filtered.ToArray();
    }

    private bool IsInSight(GameObject target, string targetLayer, string targetTag, Vector3 startPosition)
    {
        var layerMask = LayerMask.GetMask(targetLayer, "Blocking");
        var direction = Direction(target, startPosition);
        var hit = Physics2D.Raycast(startPosition, direction * 1000, 1000, layerMask);
        return hit.transform.CompareTag(targetTag);
    }

    private Vector3 Direction(GameObject target, Vector3 startPosition)
    {
        return (target.transform.position - startPosition).normalized;
    }

    public abstract CharacterAction[] GetActions();
}
