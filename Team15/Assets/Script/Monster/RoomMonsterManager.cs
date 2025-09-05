using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMonsterManager : MonoBehaviour
{
    [Header("전멸 감시 대상 루트")]
    [SerializeField] Transform monstersRoot;

    [Header("보상 UI 연결")]
    [SerializeField] RewardUI rewardUI;

    [Header("현재 방 정보(보상 분기용)")]
    [SerializeField] MapType roomType = MapType.Normal;
    [SerializeField] bool hasNextStage = true;

    int alive;
    bool opened;

    void Awake()
    {
        if (!rewardUI) rewardUI = FindObjectOfType<RewardUI>(true);

        if (!monstersRoot) monstersRoot = transform.Find("Monster");
    }

    void Start()
    {
        if (!rewardUI)
        {
            return;
        }

        if (MapSelectUI.Instance != null)
        {
            roomType = MapSelectUI.Instance.GetCurrentNodeTypeSafe();
            hasNextStage = MapSelectUI.Instance.HasNextFromCurrent();
        }

        rewardUI.Configure(roomType, hasNextStage);

        if (!monstersRoot)
        {
            ShowIfNotOpened();
            return;
        }

        var monsters = monstersRoot.GetComponentsInChildren<MonsterCondition>(true);
        alive = 0;

        foreach (var m in monsters)
        {
            if (m == null) continue;
            alive++;
            m.OnDie += HandleDie;
        }

        if (alive <= 0)
            ShowIfNotOpened();
    }

    void OnDestroy()
    {
        if (monstersRoot == null) return;
        foreach (var m in monstersRoot.GetComponentsInChildren<MonsterCondition>(true))
            if (m != null) m.OnDie -= HandleDie;
    }

    void HandleDie(MonsterCondition _)
    {
        alive--;
        if (alive <= 0)
            rewardUI.Show();
    }

    void ShowIfNotOpened()
    {
        if (opened) return;
        opened = true;
        rewardUI.Show();
    }
}

