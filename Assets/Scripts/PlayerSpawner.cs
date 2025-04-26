using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Vector3 defaultScale = new Vector3(0.03f, 0.03f, 0.03f);

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
                Vector3 spawnPos = new Vector3(markerPos.x, markerPos.y + 0.05f, markerPos.z);

                spawnedPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                string currentScene= SceneManager.GetActiveScene().name;
                float scaleFactor;
                if(currentScene=="Stage1")
                {
                    scaleFactor = 0.03f;

                }
                else
                {
                    scaleFactor = PlayerData.Instance!= null ? PlayerData.Instance.savedScale : 0.03f;
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

                // ✅ Y는 고정, XZ는 마커 따라감
                Vector3 newPos = new Vector3(markerPos.x, 0.1f, markerPos.z);
                spawnedPlayer.transform.position = newPos;
            }
        }

    }
}
