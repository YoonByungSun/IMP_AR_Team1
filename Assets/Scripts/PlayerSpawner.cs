using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public static float fixedPlayerY = 0.3f;
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

    void Start()
    {
        StartCoroutine(CheckTrackedImages());
    }

    IEnumerator CheckTrackedImages()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject room = RoomSpawner.Instance.GetRoom();
        if (room == null) yield break;

        foreach (var image in trackedImageManager.trackables)
        {
            if (image.trackingState == TrackingState.Tracking && spawnedPlayer == null)
            {
                SpawnPlayer(image, room);
                break;
            }
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (GameManager.isGameOver) return;

        GameObject room = RoomSpawner.Instance.GetRoom();
        if (room == null) return;

        foreach (var image in eventArgs.added)
        {
            if (spawnedPlayer == null && image.trackingState == TrackingState.Tracking)
                SpawnPlayer(image, room);
        }

        foreach (var image in eventArgs.updated)
        {
            if (spawnedPlayer == null && image.trackingState == TrackingState.Tracking)
                SpawnPlayer(image, room);

            if (trackedImage == image && spawnedPlayer != null)
            {
                Vector3 markerPos = image.transform.position;
                Vector3 newPos = new Vector3(markerPos.x, fixedPlayerY, markerPos.z);
                spawnedPlayer.transform.position = newPos;
            }
        }
    }

    private void SpawnPlayer(ARTrackedImage image, GameObject room)
    {
        trackedImage = image;

        Vector3 markerPos = image.transform.position;
        float yOffset = 0.05f;
        float baseY = room.transform.position.y + (room.transform.localScale.y * 0.5f) + yOffset;

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
