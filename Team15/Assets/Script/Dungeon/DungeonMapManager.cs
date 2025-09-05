using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DungeonMapManager : MonoBehaviour
{
    public static DungeonMapManager Instance { get; private set; }

    [Header("Normal 맵들")]
    [SerializeField] GameObject[] normalMaps;
    [Header("Special 맵들")]
    [SerializeField] GameObject[] specialMaps;
    [Header("Boss 맵들")]
    [SerializeField] GameObject[] bossMaps;

    [Header("현재 활성화된 맵 부모")]
    [SerializeField] Transform mapRoot;

    [Header("옵션: 플레이어/카메라/스폰 포인트 기본 이름")]
    [SerializeField] CameraController cameraController;
    [SerializeField] string wallTag = "MapEndWall";
    [SerializeField] string fallbackSpawnName = "SpawnPoint";

    GameObject currentMap;
    public GameObject CurrentMap => currentMap;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void LoadMap(MapType type) => LoadMap(type, -1, null);

    public void LoadMap(MapType type, int index, string spawnId)
    {
        if (currentMap != null) Destroy(currentMap);

        var prefab = PickPrefab(type, index);
        if (!prefab) { Debug.LogWarning($"[DungeonMapManager] No prefab for {type}"); return; }

        currentMap = Instantiate(prefab, mapRoot);
        currentMap.transform.localPosition = Vector3.zero;

        // 1) 플레이어 스폰 이동
        SnapPlayerToSpawn(currentMap, spawnId);

        // 2) 카메라 경계 재계산
        cameraController?.RecalculateBounds();
    }

    GameObject PickPrefab(MapType type, int index)
    {
        var arr = type switch
        {
            MapType.Normal => normalMaps,
            MapType.Special => specialMaps,
            MapType.Boss => bossMaps,
            _ => null
        };
        if (arr == null || arr.Length == 0) return null;
        if (index >= 0 && index < arr.Length) return arr[index];

        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    void SnapPlayerToSpawn(GameObject map, string spawnId)
    {
        var player = CharacterManager.Instance?.Player;
        if (!player) return;

        Transform spawn = null;

        var room = map.GetComponent<DungeonRoom>();
        if (room)
            spawn = string.IsNullOrEmpty(spawnId) ? room.defaultSpawn : room.GetSpawnById(spawnId);

        if (!spawn)
        {
            var t = map.transform.Find(fallbackSpawnName);
            if (t) spawn = t;
        }

        if (spawn)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb) rb.velocity = Vector2.zero;
            player.transform.position = spawn.position;
        }
        else
        {
            Debug.LogWarning("[DungeonMapManager] Spawn point not found.");
        }
    }
}
