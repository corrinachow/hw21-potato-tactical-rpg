using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public string targetTag = "PracticeTarget";

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
        var layerMask = LayerMask.GetMask(targetTag, "Blocking");

        // Get all the practice targets and send a raycast to each to see if
        // they are in line-of-sight. If they are, shoot a real fireball
        var practiceTargets = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (var practiceTarget in practiceTargets)
        {
            var direction = (practiceTarget.transform.position - firePoint.position).normalized;
            var hits = Physics2D.RaycastAll(firePoint.position, direction * 1000, 1000, layerMask);

            // debug purposes only
            Debug.DrawRay(firePoint.position, direction * 1000);

            // The raycast indicates all the colliders it hit within the layer mask
            // so we check the index of the hits, if it hits a Blocking item before it
            // hits our target, then we know it's not in sight. If it hits our target
            // before it hits a wall or something blocking, we know it's in sight.
            var indexOfBlocking = Array.FindIndex(hits, hit => LayerMask.LayerToName(hit.transform.gameObject.layer) == "Blocking");
            var indexOfTarget = Array.FindIndex(hits, hit => LayerMask.LayerToName(hit.transform.gameObject.layer) == targetTag);

            // We check for -1 because if no blocking elements were found, that is good too
            if (indexOfTarget < indexOfBlocking || indexOfBlocking == -1)
            {
                GameObject go = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity) as GameObject;
                go.SendMessage("Fire", direction);
            }
        }
    }
}
