using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Team teamToDamage;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public string targetTag = "RedTeam";
    public string targetLayer = "RedTarget";

    private Vector3 Direction(GameObject target)
    {
        return (target.transform.position - firePoint.position).normalized;
    }

    public bool IsInSight(GameObject target)
    {
        var layerMask = LayerMask.GetMask(targetLayer, "Blocking");
        var direction = Direction(target);
        var hit = Physics2D.Raycast(firePoint.position, direction * 1000, 1000, layerMask);
        return hit.transform.CompareTag(targetTag);
    }
    
    public void Shoot(GameObject target, GameObject bulletPrefab2)
    {
        GameObject go = Instantiate(bulletPrefab2, firePoint.position, Quaternion.identity) as GameObject;
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.teamToDamage = teamToDamage;
        go.SendMessage("Fire", Direction(target));
    }
}
