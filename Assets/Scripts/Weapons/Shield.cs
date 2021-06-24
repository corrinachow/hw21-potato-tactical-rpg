using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Weapon
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

    protected void OnTriggerEnter2D(Collider2D coll){
        if(coll.tag == "Fighter"){
 
            Buff buff = new Buff {
                Strength = 50,
                Magic = 0,
                Health = 0,
            };

            coll.SendMessage("ReceiveBuff", buff);

            Debug.Log("Shield " + coll.name);
        }
    }

    protected override void Action(){
        if(Input.GetKeyDown(KeyCode.E)){
            OpenShield();
        }
    }

    private void OpenShield(){
        animator.SetBool("isShieldUp", true);
        Debug.Log("Shield Open!!");
    }
}
