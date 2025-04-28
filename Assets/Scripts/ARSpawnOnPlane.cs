using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARSpawnOnPlane : MonoBehaviour
{
    public GameObject spawnPrefab; // 터치 시 생성할 오브젝트 프리팹
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool hasSpawned = false;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (hasSpawned) return;
        
            
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    Vector3 adjustedPosition= new Vector3(
                        hitPose.position.x,
                        hitPose.position.y-0.05f,
                        hitPose .position.z);
                    GameObject room= Instantiate(spawnPrefab, adjustedPosition, hitPose.rotation);
                    DontDestroyOnLoad(room);
                    hasSpawned = true;
                }
            }
        }
    }
}
