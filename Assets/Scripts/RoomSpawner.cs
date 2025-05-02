using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

// Function: Spawn Room
public class RoomSpawner : MonoBehaviour
{
    public static RoomSpawner Instance;

    public GameObject roomPrefab;
    public bool isSpawned = false;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private ARAnchorManager anchorManager;
    private GameObject room;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        anchorManager = GetComponent<ARAnchorManager>();
    }

    void Update()
    {
        if (isSpawned || Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                TrackableId planeId = hits[0].trackableId;
                ARPlane hitPlane = planeManager.GetPlane(planeId);

                if (hitPlane != null)
                {
                    ARAnchor anchor = anchorManager.AttachAnchor(hitPlane, hitPose);
                    if (anchor != null && !UIManager.Instance.IsOverUI(touch.position))
                    {
                        room = Instantiate(roomPrefab, anchor.transform);
                        room.transform.localPosition = Vector3.zero;
                        room.transform.localRotation = Quaternion.identity;

                        DontDestroyOnLoad(room);
                        isSpawned = true;

                        Debug.Log("Room Spawned with Anchor at " + hitPose.position);

                        // Disable Plane Visualization
                        foreach (ARPlane plane in planeManager.trackables)
                            DisableVisualizer(plane);
                    }
                    else Debug.LogWarning("Anchor spawn failed.");
                }
            }
        }
    }
    void OnEnable()
    {
        if (planeManager != null)
            planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        if (planeManager != null)
            planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!isSpawned) return;

        foreach (ARPlane plane in args.added)
            DisableVisualizer(plane);
    }

    private void DisableVisualizer(ARPlane plane)
    {
        plane.GetComponent<MeshRenderer>().enabled = false;
        plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
    }

    public int GetPlaneCount()
    {
        if (planeManager == null) return 0;
        return planeManager.trackables.count;
    }

    public GameObject GetRoom()
    {
        if (!isSpawned) Debug.LogError("Room not created yet.");
        return room;
    }
}