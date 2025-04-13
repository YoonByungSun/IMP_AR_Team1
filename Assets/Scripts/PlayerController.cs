using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlayerTracker : MonoBehaviour
{
    public GameObject playerPrefab;
    public Spawner spawner;
    public FieldPlacer fieldPlacer;

    private bool isGameStarted = false;
    private ARTrackedImageManager imageManager;

    void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
    }

    void Update()
    {
        if (isGameStarted) return;
        if (imageManager == null || fieldPlacer == null || spawner == null) return;

        GameObject field = fieldPlacer.GetSpawnedField();
        if (field == null) return;

        foreach (var trackedImage in imageManager.trackables)
        {
            if (trackedImage.trackingState == TrackingState.Tracking &&
                trackedImage.referenceImage.name == "player-marker")
            {
                Vector3 spawnPos = trackedImage.transform.position;
                GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                isGameStarted = true;

                spawner.StartSpawning(player.transform);
                break;
            }
        }
    }
}
