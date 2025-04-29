using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public static float fixedPlayerY = 0.3f; // 💡 enemy들도 이 Y값 참조

    private GameObject spawnedPlayer;
    private ARTrackedImage trackedImage;
    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
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
            if (spawnedPlayer == null)
            {
                trackedImage = addedImage;

                Vector3 markerPos = trackedImage.transform.position;

                // ✅ Room이 있으면 Room 위에 생성
                GameObject room = GameObject.Find("Room(Clone)");
                float yOffset = 0.05f;
                float baseY = markerPos.y;

                if (room != null)
                {
                    baseY = room.transform.position.y + (room.transform.localScale.y * 0.5f) + yOffset;
                }

                fixedPlayerY = baseY;
                Vector3 spawnPos = new Vector3(markerPos.x, fixedPlayerY, markerPos.z);

                spawnedPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

                string currentScene = SceneManager.GetActiveScene().name;
                float scaleFactor;

                if (PlayerData.Instance != null)
                {
                    PlayerData.Instance.SetScaleForStage(currentScene);
                    scaleFactor = PlayerData.Instance.savedScale;
                }
                else
                {
                    scaleFactor = 0.03f;
                }

                spawnedPlayer.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                spawnedPlayer.tag = "Player";

                Debug.Log("✅ 플레이어 생성 완료: " + spawnPos);
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
