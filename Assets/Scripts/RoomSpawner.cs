using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

// Function: Spawn Room
public class RoomSpawner : MonoBehaviour
{
    public GameObject roomPrefab;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private ARAnchorManager anchorManager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool hasSpawned = false;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        anchorManager = GetComponent<ARAnchorManager>();
    }

    void Update()
    {
        if (hasSpawned || Input.touchCount == 0)
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
                    if (anchor != null)
                    {
                        GameObject room = Instantiate(roomPrefab, anchor.transform);
                        room.transform.localPosition = Vector3.zero;
                        room.transform.localRotation = Quaternion.identity;

                        DontDestroyOnLoad(room);
                        hasSpawned = true;

                        Debug.Log("✅ Room Spawned with Anchor at " + hitPose.position);
                    }
                    else
                    {
                        Debug.LogWarning("❌ Anchor 생성 실패");
                    }
                }
            }
        }
    }
}