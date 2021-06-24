using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damageAmount = 5;
    public float pushForce = 2f;
    public Rigidbody2D rb;

    public Team teamToDamage;

    void Fire(Vector3 position)
    {
        rb.velocity = position * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Character enemy = collision.GetComponent<Character>();

        if (enemy != null && enemy.Team == teamToDamage)
        {
            Damage dmg;
            dmg.damageAmount = damageAmount;
            dmg.pushForce = pushForce;
            dmg.origin = new Vector3();
            enemy.ReceiveDamage(dmg);
            Destroy(gameObject);
        }

        // Destroy bullet after 10 seconds if it doesn't hit anything
        Destroy(gameObject, 10f);
    }
}
