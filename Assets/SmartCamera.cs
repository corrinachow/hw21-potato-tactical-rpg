using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SmartCamera : MonoBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera playerCamera;

    [SerializeField]
    private CinemachineVirtualCamera overviewCamera;

    [SerializeField]
    private GameObject overviewCameraMoveTarget;

    [SerializeField]
    private GameObject secondPlayer;

    public int cameraSpeed = 10;

    public int zoomSpeed = 5;

    private Vector3 moveDelta;

    enum Modes
    {
        player,
        overview
    }

    private Modes mode = Modes.player;

    // Start is called before the first frame update
    void Start()
    {
        if (overviewCamera.Follow == null)
        {
            Debug.Log("Warning, the overview camera should be following a gameobject");
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
            Input's are for debugging, these should be removed once the Game Manager is in
        */
        if (Input.GetKeyDown("o"))
        {
            Debug.Log("Switching to Overview cam");
            SetToOverviewCamera();
        }

        if (Input.GetKeyDown("p"))
        {
            Debug.Log("Switching to Player cam");
            SetToPlayerCamera();
        }

        if (Input.GetKeyDown("k"))
        {
            Debug.Log("Focusing on second player");
            FocusOn(secondPlayer);
        }

        if (mode == Modes.overview)
        {
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");

            moveDelta = new Vector3(x, y, 0);

            overviewCameraMoveTarget.transform.Translate(
                moveDelta.x * Time.deltaTime * cameraSpeed,
                moveDelta.y * Time.deltaTime * cameraSpeed,
                0
            );


            // Don't allow for zooming in too much
            float nextZoom = overviewCamera.m_Lens.OrthographicSize + Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;

            if (nextZoom > 0.1)
            {
                overviewCamera.m_Lens.OrthographicSize = nextZoom;
            }
        }
    }

    public void SetToOverviewCamera()
    {
        overviewCameraMoveTarget.transform.position = playerCamera.Follow.position;
        overviewCamera.MoveToTopOfPrioritySubqueue();
        mode = Modes.overview;
    }

    public void SetToPlayerCamera()
    {
        playerCamera.MoveToTopOfPrioritySubqueue();
        mode = Modes.player;
    }

    public void FocusOn(GameObject gameObject)
    {

        playerCamera.Follow = gameObject.transform;
        playerCamera.MoveToTopOfPrioritySubqueue();
    }
}
