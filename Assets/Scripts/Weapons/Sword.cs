using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{

     // Damage struct
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public int Attack { get; } = 25;
    public int Strength { get; } = 10;

    protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void OnCollide(Collider2D coll){
        
    }

    protected void OnTriggerEnter2D(Collider2D coll){
        if(coll.tag == "Fighter"){
            if(coll.name=="Player"){
                return;
            }

            Damage dmg = new Damage {
                damageAmount = (int)((Attack * Random.Range(1,1.125f))*(1+(Strength*Strength/256))),
            };

            coll.SendMessage("ReceiveDamage", dmg);

            Debug.Log("sword: "+coll.name);
            Debug.Log(dmg.damageAmount);

        }
    }

    protected override void Action(){
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Swing();
        }

    }

    public void Swing(){
        animator.SetTrigger("Swing_Sword");
    }
}
