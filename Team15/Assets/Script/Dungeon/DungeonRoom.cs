using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [Header("Spawn")]
    public Transform defaultSpawn;

    public Transform GetSpawnById(string id)
    {
        foreach (var m in GetComponentsInChildren<RoomSpawnMarker>(true))
            if (m != null && m.id == id) return m.transform;
        return defaultSpawn;
    }

    public class RoomSpawnMarker : MonoBehaviour
    {
        public string id = "Entry";
    }
}
