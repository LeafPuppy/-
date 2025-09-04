using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapManager : MonoBehaviour
{
    [Header("Normal 맵들")]
    [SerializeField] GameObject[] normalMaps;
    [Header("Special 맵들")]
    [SerializeField] GameObject[] specialMaps;
    [Header("Boss 맵들")]
    [SerializeField] GameObject[] bossMaps;

    [Header("현재 활성화된 맵 부모")]
    [SerializeField] Transform mapRoot;

    GameObject currentMap;

    public void LoadMap(MapType type)
    {
        if (currentMap != null) Destroy(currentMap);

        GameObject prefab = null;

        switch (type)
        {
            case MapType.Normal:
                prefab = normalMaps[Random.Range(0, normalMaps.Length)];
                break;
            case MapType.Special:
                prefab = specialMaps[Random.Range(0, specialMaps.Length)];
                break;
            case MapType.Boss:
                prefab = bossMaps[Random.Range(0, bossMaps.Length)];
                break;
        }

        if (prefab != null)
        {
            currentMap = Instantiate(prefab, mapRoot);
            currentMap.transform.localPosition = Vector3.zero;
        }
    }
}
