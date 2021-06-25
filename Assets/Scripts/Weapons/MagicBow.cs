using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBow : Weapon
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


    protected override void Action(){
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ShootArrow();
        }

    }

    public void ShootArrow(){
        animator.SetTrigger("ShootMagicArrow");
    }
}
