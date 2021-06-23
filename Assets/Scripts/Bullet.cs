using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0.0001f;
    public int damageAmount = 5;
    public float pushForce = 2f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            Damage dmg;
            dmg.damageAmount = damageAmount;
            dmg.pushForce = pushForce;
            dmg.origin = new Vector3();
            enemy.ReceiveDamage(dmg);
        }

        Destroy(gameObject, 2f);
    }
}
