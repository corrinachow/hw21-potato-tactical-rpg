using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hitpoint = 10;
    public int maxHitpoint = 10;
    public float pushREcoverySpeed = 0.2f;

    //immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    //push
    protected Vector3 pushDirection;

    //All fighters
    //recieveDamage & Die
    protected virtual void ReceiveDamage(Damage dmg){
        if(Time.time - lastImmune > immuneTime){
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            Debug.Log(dmg.damageAmount);

            if (hitpoint <= 0){
                hitpoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death(){

    }
}
