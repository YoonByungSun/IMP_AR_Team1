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
        if (hasSpawned || Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        // ✅ 터치가 시작될 때만 반응
        if (touch.phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                // ✅ 위치를 아래로 약간 내림 (Room이 평면 아래 묻히지 않도록)
                Vector3 adjustedPosition = new Vector3(
                    hitPose.position.x,
                    hitPose.position.y ,  // 원래 -0.2f는 너무 낮을 수도 있어 조정 가능
                    hitPose.position.z
                );

                GameObject room = Instantiate(spawnPrefab, adjustedPosition, hitPose.rotation);

                DontDestroyOnLoad(room); // ✅ 씬 전환 시 유지
                hasSpawned = true;

                Debug.Log("✅ Room Spawned at " + adjustedPosition);
            }
        }
    }
}
