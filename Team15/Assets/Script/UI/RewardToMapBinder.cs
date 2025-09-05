using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardToMapBinder : MonoBehaviour
{
    [SerializeField] RewardUI rewardUI;
    [SerializeField] MapSelectUI mapUI;
    [SerializeField] int floor = 1;

    void OnEnable() { if (rewardUI) rewardUI.OnFinished += OpenMap; }
    void OnDisable() { if (rewardUI) rewardUI.OnFinished -= OpenMap; }

    void OpenMap()
    {
        Debug.Log("[Binder] OpenMap 호출됨");
        mapUI.Open(floor);
    }
}
