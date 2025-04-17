using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public GameObject prefab;
    }

    public List<ItemData> itemList;           // 스폰할 수 있는 아이템 목록
    private float spawnIntervalMin = 1.5f;       // 최소 스폰 시간
    private float spawnIntervalMax = 5f;       // 최대 스폰 시간
    private float spawnRadius = 5.0f;          // 플레이어 기준 스폰 반경

    //private Transform player;
    public GameObject player;

    void Start()
    {
        Debug.Log(player.transform.position);
    }

    IEnumerator SpawnItemRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        if (itemList.Count == 0 || player == null) return;

        // 랜덤 아이템 선택
        ItemData selectedItem = itemList[Random.Range(0, itemList.Count)];

        // 플레이어 주변 랜덤 위치 계산
        Vector3 randomOffset = Random.onUnitSphere * spawnRadius;
        randomOffset.y = 0f;
        Vector3 spawnPos = player.transform.position + randomOffset;

        // 아이템 생성
        Instantiate(selectedItem.prefab, spawnPos, Quaternion.identity, this.transform);
    }
}
