using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public static float fixedPlayerY = 0.3f; // 💡 enemy들도 이 Y값 참조
    public bool isSpawned = false;

    private GameObject spawnedPlayer;
    private ARTrackedImage trackedImage;
    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = FindAnyObjectByType<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var addedImage in eventArgs.added)
        {
            GameObject room = RoomSpawner.Instance.GetRoom();
            if (spawnedPlayer == null && room != null)
            {
                trackedImage = addedImage;

                Vector3 markerPos = trackedImage.transform.position;

                float yOffset = 0.05f;
                float baseY = markerPos.y;

                if (room != null)
                {
                    baseY = room.transform.position.y + (room.transform.localScale.y * 0.5f) + yOffset;
                }

                fixedPlayerY = baseY;
                Vector3 spawnPos = new Vector3(markerPos.x, fixedPlayerY, markerPos.z);

                spawnedPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

                float scale = 0.015f;
                spawnedPlayer.transform.localScale = new Vector3(scale, scale, scale);
                spawnedPlayer.tag = "Player";
                spawnedPlayer.transform.parent = new GameObject("Player").transform;
                PlayerController.scale = scale;

                Debug.Log("Player Spawned at " + spawnPos);
                isSpawned = true;
            }
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            if (trackedImage == updatedImage && spawnedPlayer != null)
            {
                Vector3 markerPos = updatedImage.transform.position;

                // ✅ XZ는 마커 따라가고, Y는 고정된 값 유지
                Vector3 newPos = new Vector3(markerPos.x, fixedPlayerY, markerPos.z);
                spawnedPlayer.transform.position = newPos;
            }
        }
    }
}
