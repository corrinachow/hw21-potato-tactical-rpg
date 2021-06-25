using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Camera cam;

    private NavMeshAgent agent;

    private LineRenderer myLineRenderer;

    public Tilemap map;

    public GameObject moveTarget;

    public GameObject maxMoveCircle;

    public int maxMoveDistance = 5;

    public bool isDebug = false;

    public bool isFacingLeftAtStart = false;

    // private BoxCollider2D boxCollider;
    private Vector3 lastPosition = new Vector3(0, 0, 0);

    private bool clicked = false;


    private void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Line rendere for destination line
        myLineRenderer.startWidth = 0.05f;
        myLineRenderer.endWidth = 0.05f;
        myLineRenderer.positionCount = 0;

        lastPosition = transform.position;
    }

    public void DisplayMoveCircle()
    {
        maxMoveCircle.SetActive(true);
    }

    /* 
        Moves to the set destination, invoke TargetPoint with the mousePosition prior to this
    */
    public void MoveToDestination()
    {
        maxMoveCircle.SetActive(false);
        moveTarget.transform.SetParent(null);
        agent.isStopped = false;
    }

    /*
        Draws line and point to the desired destination, confirm by invoking MoveToDestination()
    */
    public void TargetPoint(Vector2 mousePosition)
    {
        moveTarget.SetActive(true);
        moveTarget.transform.position = mousePosition;
        agent.isStopped = true;
        agent.SetDestination(mousePosition);
    }

    /*
    Example of how to check of the point exists.
    */
    private void ClickToMove()
    {
        if (!clicked)
        {
            DisplayMoveCircle();
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = cam.ScreenToWorldPoint(mousePosition);

            if (Vector2.Distance(transform.position, mousePosition) <= maxMoveDistance)
            {
                Vector3Int gridPosition = map.WorldToCell(mousePosition);
                if (map.HasTile(gridPosition))
                {
                    TargetPoint(mousePosition);
                }
            }
            else
            {
                Debug.Log("That point is too far!");
            }
        }
        else
        {
            MoveToDestination();
        }
        clicked = !clicked;
    }

    private void FixedUpdate()
    {
        if (isDebug && Input.GetMouseButtonDown(0))
        {
            ClickToMove();
        }

        if (Vector3.Distance(agent.destination, transform.position) <= agent.stoppingDistance)
        {
            // If the agent is moving
            moveTarget.SetActive(false);
            myLineRenderer.positionCount = 0;
        }
        else if (agent.hasPath)
        {
            // If the agent has a path
            DrawPath();
        }

        Vector3 moveDelta = transform.position - lastPosition;
        lastPosition = transform.position;

        if (isFacingLeftAtStart)
        {
            moveDelta.x *= -1;
        }

        // Swap sprite direction when going left or right
        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 0);
    }

    void DrawPath()
    {
        // Draws the path that the player will take to reach it's destination
        myLineRenderer.positionCount = agent.path.corners.Length;
        myLineRenderer.SetPosition(0, transform.position);

        // Straight line
        if (agent.path.corners.Length < 2)
        {
            return;
        }

        for (int i = 1; i < agent.path.corners.Length; i++)
        {
            Vector3 pointPosition = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y, agent.path.corners[i].z);
            myLineRenderer.SetPosition(i, pointPosition);
        }
    }
}
