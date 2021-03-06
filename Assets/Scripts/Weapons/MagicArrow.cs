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

    }

    protected void OnTriggerEnter2D(Collider2D coll){
        //DMG = ([25 x RANDOM(1~1.125)] x [2 + MAG x MAG/256)])  * 0.8 * ((100-MDEF) /100)
    }


    protected override void Action(){
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ShootArrow();
        }

    }

    private void ShootArrow(){
        // animator.SetTrigger("ShootMagicArrow");
    }
}
