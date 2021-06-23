using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;

    public Tilemap map;

    // private BoxCollider2D boxCollider;
    private Vector3 lastPosition = new Vector3(0, 0, 0);


    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = cam.ScreenToWorldPoint(mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            if (map.HasTile(gridPosition))
            {
                agent.SetDestination(mousePosition);
            }
        }

        Vector3 moveDelta = transform.position - lastPosition;
        lastPosition = transform.position;

        // // Swap sprite direction when going left or right
        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 0);

    }
}
