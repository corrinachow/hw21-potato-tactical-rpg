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

    protected override void OnCollide(Collider2D coll){
        if(coll.tag == "Fighter"){
 
            Buff buff = new Buff {
                isShieldOpen = true,
                hasManaward = false,
                hasBarrier= false,
                health = 0,
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
        animator.SetBool("isShieldOpen", true);
        Debug.Log("Shield Open!!");
    }
}
