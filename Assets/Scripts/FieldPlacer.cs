using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class FieldPlacer : MonoBehaviour
{
    public GameObject fieldPrefab;
    private GameObject spawnedField;

    private ARPlaneManager planeManager;
    private bool isFieldPlaced = false;

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        if (isFieldPlaced || planeManager == null) return;

        foreach (ARPlane plane in planeManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp && plane.trackingState == TrackingState.Tracking)
            {
                spawnedField = Instantiate(fieldPrefab, plane.transform.position, Quaternion.identity);
                isFieldPlaced = true;
                Debug.Log("✅ Room 프리팹이 자동으로 생성되었습니다!");
                break;
            }
        }
    }

    public GameObject GetSpawnedField()
    {
        return spawnedField;
    }
}
