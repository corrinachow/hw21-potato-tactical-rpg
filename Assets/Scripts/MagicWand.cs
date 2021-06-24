using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public string targetTag = "RedTeam";
    public string targetLayer = "RedTarget";

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot ()
    {
        // A layer mask to get only Blocking elements and targetTag elements
        var layerMask = LayerMask.GetMask(targetLayer, "Blocking");

        // Get all the targets and send a raycast to each to see if
        // they are in line-of-sight. If they are, shoot a real fireball
        var targets = GameObject.FindGameObjectsWithTag(targetTag);

        Debug.Log("Targets: " + targets.Length);
        Debug.Log(targetTag);

        foreach (var target in targets)
        {
            var direction = (target.transform.position - firePoint.position).normalized;
            var hit = Physics2D.Raycast(firePoint.position, direction * 1000, 1000, layerMask);

            Debug.DrawRay(firePoint.position, direction * 10_000, Color.red, 10f);
            Debug.Log("Tag hit: " + hit.transform.tag + " | Name: " + hit.transform.name);

            if (hit.transform.CompareTag(targetTag))
            {
                GameObject go = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity) as GameObject;
                go.SendMessage("Fire", direction);
            }
        }
    }
}
