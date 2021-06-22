using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : Fighter
{
    private BoxCollider2D boxCollider;
    //players currently position with the move delta and end up where we want to be
    protected Vector3 moveDelta;
    //checks the player hitbox
    protected RaycastHit2D hit;
    protected float ySpeed = 0.75f;
    protected float xSpeed = 1.0f;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }


    protected virtual void UpdateMotor(Vector3 input){
        //Reset moveDelta ()
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //swap sprite direction
        if(moveDelta.x>0){
            transform.localScale= Vector3.one;
        }
        else if (moveDelta.x<0){
            transform.localScale = new Vector3(-1,1,1);
        }

        //Make sure we can move in this direction by casting in this direction --> Y axis
        hit = Physics2D.BoxCast(transform.position,boxCollider.size, 0,new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null){
            //Move player
            transform.Translate(0, moveDelta.y * Time.deltaTime,0);
        }

        //Make sure we can move in this direction by casting in this direction --> X-axis
        hit = Physics2D.BoxCast(transform.position,boxCollider.size, 0,new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null){
            //Move player
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
