using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrow : Weapon
{

     // Damage struct
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public int Attack { get; } = 25;
    public int Magic { get; } = 15;

    protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void OnCollide(Collider2D coll){
        if(coll.tag == "Fighter"){
            if(coll.name=="Player"){
                return;
            }
//DMG = [25 x RANDOM(1~1.125) - MDEF] x [2 + MAG x MAG/256)]  * 0.8

            Damage dmg = new Damage {
                damageAmount = (int)((Attack * Random.Range(1,1.125f))*(2+(Magic*Magic/256))),
            };

            coll.SendMessage("ReceiveMagicDamage", dmg);

            Debug.Log(coll.name);
            Debug.Log(dmg.damageAmount);

        }
    }

    protected override void Action(){
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Swing();
        }

    }

    private void Swing(){
        animator.SetTrigger("Swing_Sword");
    }
}
